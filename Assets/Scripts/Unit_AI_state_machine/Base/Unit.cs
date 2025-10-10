using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
using System;

// Units are state machine based and are not connected to a goap agent.
// The faction's goap agent gives orders to one or more units.
// Check out the GoapAgent class to understand how orders are given.

public class Unit : Target, IDamageable, ISelectable_Character, IAttackable_Character
{
    public SkinnedMeshRenderer meshRenderer;
    public Camera player_cam;
    public NavMeshAgent agent;
    public Animator animator;
    protected Color unit_color = Color.green;
    public LayerMask ground;
    [SerializeField] public LayerMask[] layers;
    public float min_attack_distance {  get; set; }
    public float max_patroling_distance;
    public Unit commando_unit = null;
    public float patroling_speed;
    public int min_follow_distance;
    public GameObject target = null;
    public HashSet<GameObject> targets_in_range = new HashSet<GameObject>();
    public bool follow_mouse_click_position = false;

    public TextMeshProUGUI state_label;
    [SerializeField] private float action_time;
    public bool is_patroling = false;
    [SerializeField] private float min_building_attack_Distance;
    [SerializeField] private float min_character_attack_distance;

    #region State Machine Variables

    public Unit_StateMachine stateMachine {  get; set; }
    public Unit_Idle_State unit_idle_state { get; set; }
    public Unit_Attack_State unit_attack_state { get; set; }
    public Unit_Chase_State unit_chase_state { get; set; }
    public Unit_Walk_State unit_walk_state { get;  set; }
    public Player_Unit_Return_To_Commander_State unit_return_to_commander_state { get; set; }
    public Unit_Die_State unit_die_state { get; set; }
    public Unit_Patroling_State unit_patrol_state { get; set; }
    public bool is_seeing_opponent { get; set; }
    public bool is_opponent_in_attack_range { get; set; }
    #endregion

    [Header("Health")]
    [field: SerializeField] public float max_health { get; set; }
    [field: SerializeField] public float current_health { get; set; }
    [Header("Damage")]
    [field: SerializeField] public float damage { get; set; }
    public enum animation_trigger_type
    {
        unit_attacks,
        unit_attack_finished,
        unit_damaged = 10,
        play_footstep_sounds = 30,
    }

    private void OnEnable()
    {
        removeUnitFromList += OnTargetRemovedFromList;
    }


    private void OnDisable()
    {
        removeUnitFromList -= OnTargetRemovedFromList;
    }

    public virtual void OnAwake()
    {

    }

    private void OnTargetRemovedFromList(GameObject potentialDeadTarget)
    {
        if(potentialDeadTarget == target)
        {
            target = null;
        }
    }

    private void Awake()
    {
        stateMachine = new Unit_StateMachine();
        unit_idle_state = new Unit_Idle_State(this, stateMachine);
        unit_chase_state = new Unit_Chase_State(this, stateMachine);
        unit_attack_state = new Unit_Attack_State(this, stateMachine);
        unit_walk_state = new Unit_Walk_State(this, stateMachine);
        unit_return_to_commander_state = new Player_Unit_Return_To_Commander_State(this, stateMachine);
        unit_die_state = new Unit_Die_State(this, stateMachine);
        unit_patrol_state = new Unit_Patroling_State(this, stateMachine);
    }

    void Start()
    {
        init_unit_color();
        onStart();
        Unit_Selections.instance.unit_list.Add(this.gameObject);
        current_health = max_health;
        player_cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        newTargetAvailable?.Invoke(gameObject, faction);
    }

    public void animation_trigger_event(animation_trigger_type trigger_Type)
    {
        stateMachine.current_unit_state.animation_trigger_event(trigger_Type);
        
    }

    private void Update()
    {
        
        stateMachine.current_unit_state.frame_update();
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
    }

    public void enable_selection_material(bool enable)
    {
        if (enable) { 
            meshRenderer.material.color = Color.blue;
        }
        else 
        {
            meshRenderer.material.color = unit_color;
        }
    }

    public void update_health(float increment, Unit attacker)
    {

        if (attacker != null && attacker.gameObject != target) {
            target = attacker.gameObject;
           
        }

        current_health += increment;
        if (current_health <= 0f)
        {
           stateMachine.change_state(unit_die_state);
        }
    }

    public void opponent_entered_view_range(Unit opponent)
    {
        target = opponent.gameObject;
        stateMachine.change_state(unit_chase_state);
    }

    public void opponent_exited_view_range(Unit opponent)
    {
        target = null;
        stateMachine.change_state(unit_walk_state);
        
    }

    public void opponent_entered_attack_range()
    {
        stateMachine.change_state(unit_attack_state);
    }

    public void opponent_exited_attack_range()
    {
        stateMachine.change_state(unit_chase_state);
    }

    public void set_nav_agent_destination_and_target(Vector3 new_destination, GameObject new_target)
    {
        if (new_target != commando_unit)
        {
            target = new_target.gameObject;
            AdjustAttackDistanceToTarget();
            stateMachine.change_state(unit_chase_state);
        }
        else
        {
            target = commando_unit.gameObject;
            agent.stoppingDistance = min_follow_distance;
            stateMachine.change_state(unit_walk_state);
        }

        agent.SetDestination(new_destination);

    }

    private void AdjustAttackDistanceToTarget()
    {
        if (target.TryGetComponent(out Building building))
        {
            min_attack_distance = min_building_attack_Distance;
            agent.stoppingDistance = min_attack_distance;
        }
        else
        {
            min_attack_distance = min_character_attack_distance;
            agent.stoppingDistance = min_attack_distance;
        }
    }

    public void find_closest_target_in_range()
    {
        if (targets_in_range == null || targets_in_range.Count == 0) return;
        var shortest_distance_to_target = math.INFINITY;
        var distance_to_target = 0f;
        GameObject closest_target = null;

        foreach (var target in targets_in_range)
        {
            if (target == null || (target.TryGetComponent<Target>(out Target targetComponent) && targetComponent.dead))
            {
                 continue;
            }

            distance_to_target = Mathf.Abs((target.transform.position - transform.position).magnitude);
            if (distance_to_target < shortest_distance_to_target)
            {
                shortest_distance_to_target = distance_to_target;
                closest_target = target;
            }

        }

        if (closest_target != null && (closest_target.TryGetComponent<Target>(out Target _targetComponent) && !_targetComponent.dead))
        {
            target = closest_target;
        }else
        {
            targets_in_range.Clear();
        }
    }
    public void die()
    {
        removeUnitFromList?.Invoke(gameObject);
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        Unit_Selections.instance.unit_list.Remove(this.gameObject);
    }

    public virtual void onStart()
    {
        if (is_patroling)
        {
            stateMachine.initialize(unit_patrol_state);
        }
        else
        {
            stateMachine.initialize(unit_walk_state);
        }
    }

    private void init_unit_color()
    {
        if ( factionType == factionTypes.CPU)
        {
            unit_color = Color.red;

        }
        else if (factionType == factionTypes.player)
        {

            unit_color = meshRenderer.material.color;
          
        }
        else
        {
            unit_color = Color.yellow;
        }
    }

    public void select()
    {
        enable_selection_material(true);
        follow_mouse_click_position = true;
    }

    public void deselect()
    {
       follow_mouse_click_position = false;
       enable_selection_material(false);
    }

    public bool isSelectable()
    {
        // only player units can be selected
        return (factionType == factionTypes.player);
    }
}

