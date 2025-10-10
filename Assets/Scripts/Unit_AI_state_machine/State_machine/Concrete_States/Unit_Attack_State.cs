using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.Mathematics;
public class Unit_Attack_State : Unit_State
{
    protected float attack_time = 4f;
    protected float current_attack_time = 0f;
    public Unit_Attack_State(Unit unit, Unit_StateMachine unit_stateMachine) : base(unit, unit_stateMachine)
    {
    }
    public override void animation_trigger_event(Unit.animation_trigger_type trigger_Type)
    {
        base.animation_trigger_event(trigger_Type);
        if (trigger_Type == Unit.animation_trigger_type.unit_attacks)
        {
            attack();
        }else if (trigger_Type == Unit.animation_trigger_type.unit_attack_finished)
        {
           
        }
    }
    
    public override void enter_state()
    {
        base.enter_state();
        current_attack_time = 0f;
        unit.agent.isStopped = true;

    }

    public override void exit_state()
    {
        base.exit_state();
        current_attack_time = 0f;
        unit.enable_selection_material(false);
        unit.agent.isStopped = false;
    }

    public override void frame_update()
    {
        base.frame_update();
        if (!check_mouse_input() && unit.target != null)
        {
            if (Mathf.Abs((unit.target.transform.position - unit.transform.position).magnitude) > unit.min_attack_distance)
            {
                unit.stateMachine.change_state(unit.unit_chase_state);
            }
           
        }
        else if (unit.target == null)
        {
            if (unit.targets_in_range.Count > 0)
            {
                unit.find_closest_target_in_range();
                return;
            }
            else
            {

                unit.stateMachine.change_state(unit.unit_idle_state);
                return;
            }

        }
        current_attack_time += Time.deltaTime;
        if (current_attack_time >= attack_time)
        {
            attack();

        }
        else
        {
            unit.meshRenderer.material.color = Color.Lerp(unit.meshRenderer.material.color, Color.black, Time.deltaTime);
        }
    }
    public override void physics_update()
    {
        base.physics_update();
    }
    public override bool check_mouse_input()
    {
        return base.check_mouse_input();
    }
    public override void handle_mouse_input(Ray _ray, RaycastHit _hit)
    {
        base.handle_mouse_input(_ray, _hit);

        var potential_target = _hit.collider.GetComponent<Target>();
        if (potential_target != null && potential_target.faction != unit.faction && _hit.collider.gameObject != unit.gameObject)
        {
            unit.set_nav_agent_destination_and_target(_hit.collider.transform.position, potential_target.gameObject);
        }
        else
        {
            unit.agent.SetDestination(_hit.point);
            unit.agent.stoppingDistance = 0f;
            unit.target = null;
        }
        
    }

    public virtual void attack()
    {
        current_attack_time = 0;
        unit.enable_selection_material(false);
        unit.target.GetComponent<IDamageable>().update_health(-unit.damage, unit);
        unit.animator.SetTrigger("Attack01");
    }
}
