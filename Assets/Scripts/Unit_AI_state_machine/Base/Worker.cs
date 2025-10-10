using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Unity.Mathematics;
using System.Collections;
using System;

// Workers are state machine based and are not connected to a goap agent.
// The faction's goap agent gives orders to one or more workers.
// Check out the GoapAgent class to understand how orders are given.

public class Worker: Target, IDamageable, ISelectable_Character
{
    public SkinnedMeshRenderer meshRenderer;
    public Camera player_cam;
    public NavMeshAgent agent;
    public Animator animator;

    public float max_distance_to_workplace;
    public float min_gather_distance;

    protected Color unit_color = Color.green;
    public LayerMask ground;
    [SerializeField] public LayerMask[] layers;
    public bool follow_mouse_click_position = false;

    public bool order_placed = false;
    public GameObject target = null;
    public Workplace workplace = null;
    public HashSet<GameObject> targets_in_range = new HashSet<GameObject>();
    public TextMeshProUGUI state_label;
    [SerializeField] private float action_time;
    [SerializeField] public ResourcePack maxGatherCapacity = new ResourcePack(200, 200, 200, 100);

    [SerializeField] public ResourcePack gatheredResourcesPack = new ResourcePack();
    [SerializeField] public float offload_time { get; private set; } = 6f;

    private float current_action_time;
    #region State Machine Variables

    public Worker_StateMachine stateMachine { get; set; }
    public Worker_Die_State worker_die_state { get; set; }
    public Worker_Going_To_Worklpace worker_going_to_worklpace { get; set; }
    public Worker_Idle_State worker_idle_state { get; set; }
    public Worker_Gather_State worker_gather_state { get; set; }
    public Worker_Going_To_Resource worker_going_to_resource { get; set; }

    public Worker_Offload_State worker_offload_state { get; set; }

    public Worker_Going_To_Mouse_Click_Position worker_going_to_mouse_click_position { get; set; }
    public bool is_seeing_opponent { get; set; }
    public bool is_opponent_in_attack_range { get; set; }
    #endregion

    [Header("Health")]
    [field: SerializeField] public float max_health { get; set; }
    [field: SerializeField] public float current_health { get; set; }

    public enum animation_trigger_type
    {
        worker_started_gathering,
        unit_damaged = 10,
        play_footstep_sounds = 30,
    }

    private void Awake()
    {
        stateMachine = new Worker_StateMachine();
        worker_idle_state = new Worker_Idle_State(this, stateMachine);
        worker_going_to_resource = new Worker_Going_To_Resource(this, stateMachine);
        worker_gather_state = new Worker_Gather_State(this, stateMachine);
        worker_going_to_worklpace = new Worker_Going_To_Worklpace(this, stateMachine);
        worker_die_state = new Worker_Die_State(this, stateMachine);
        worker_going_to_mouse_click_position = new Worker_Going_To_Mouse_Click_Position(this, stateMachine);
        worker_offload_state = new Worker_Offload_State(this, stateMachine);
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
   
    private void Update()
    {
        stateMachine.current_worker_state.frame_update();
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
    }
   
    public void enable_selection_material(bool enable)
    {
        if (enable)
        {
            meshRenderer.material.color = Color.blue;
        }
        else
        {
            meshRenderer.material.color = unit_color;
        }
    }

    public void update_health(float increment, Unit attacker)
    {
        if (attacker != target)
        {
            order_placed = false;


        }
        current_health += increment;
        if (current_health <= 0f)
        {
            stateMachine.change_state(worker_die_state);
        }
    }

    public void set_nav_agent_destination_and_target(GameObject new_target)
    {
        target = new_target;
        agent.stoppingDistance = min_gather_distance;

      
        stateMachine.change_state(worker_going_to_resource);
        agent.SetDestination(new_target.transform.position);
    }
    public void find_closest_target_in_range()
    {
        if (targets_in_range == null || targets_in_range.Count == 0) return;
        var shortest_distance_to_target = math.INFINITY;
        var distance_to_target = 0f;
        GameObject closest_target = null;

        foreach (var target in targets_in_range)
        {
            if (target == null) continue;
            distance_to_target = Mathf.Abs((target.transform.position - transform.position).magnitude);
            if (distance_to_target < shortest_distance_to_target)
            {
                shortest_distance_to_target = distance_to_target;
                closest_target = target;
            }
        }

        if (closest_target != null)
        {
            target = closest_target.gameObject;
        }
        else
        {
            targets_in_range.Clear();
            faction.RemoveWorkerFromBusyList(this);
        }
    }

    public bool TryFindAppropriateWorkplace()
    {
        if (target.TryGetComponent<Resource>(out Resource resource))
        {
            var workplaces = faction.GetWorkplaces();
            foreach (var workplace in workplaces)
            {
                if (workplace.IsWorkplaceAppropriateForResource(resource))
                {
                    this.workplace = workplace;
                    return true;
                }
            }

        }
        Debug.Log("You need to construct an appropriate workplace to gather this resource : " + resource);
        return false;
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
        stateMachine.initialize(worker_idle_state);
    }
 
    private void init_unit_color()
    {
        if (factionType == factionTypes.CPU)
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

    public void offload()
    {
        faction.UpdateResourceAmount(gatheredResourcesPack);
        gatheredResourcesPack = new ResourcePack();
    }

    public void set_hold_position(bool hold_position)
    {
        if (hold_position)
        {
            hold_position = true;
        }
        else
        {
            hold_position = false;
        }
    }

    public bool get_hold_position()
    {
        return false;
    }

    public bool HasAppropriateWorkplace()
    {
        if (target.TryGetComponent<Resource>(out Resource resource) && workplace != null)
        {
            if (workplace.IsWorkplaceAppropriateForResource(resource))
            {
                return true;
            }
            
        }
        return false;
    }

    public bool isSelectable()
    {
        // only player units can be selected
        return (factionType == factionTypes.player);
    }
}

