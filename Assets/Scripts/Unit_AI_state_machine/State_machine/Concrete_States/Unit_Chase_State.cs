using UnityEngine;

public class Unit_Chase_State : Unit_State
{
    public Unit_Chase_State(Unit unit, Unit_StateMachine unit_stateMachine) : base(unit, unit_stateMachine)
    {
    }
    public override void animation_trigger_event(Unit.animation_trigger_type trigger_Type)
    {
        base.animation_trigger_event(trigger_Type);
    }
    public override void enter_state()
    {
        base.enter_state();
    }
    public override void exit_state()
    {
        base.exit_state();
    }

    public override void frame_update()
    {
        base.frame_update();

        if (!check_mouse_input() && unit.target != null)
        {
            if (Mathf.Abs((unit.target.transform.position - unit.transform.position).magnitude) > unit.min_attack_distance)
            {
                unit.set_nav_agent_destination_and_target(unit.target.transform.position, unit.target);
            }
            else
            {
                unit.stateMachine.change_state(unit.unit_attack_state);
            }

        }
        else if (unit.target == null)
        {
            if (unit.targets_in_range.Count > 0)
            {
                unit.find_closest_target_in_range();
            }
            else
            {
                unit.stateMachine.change_state(unit.unit_idle_state);
            }
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
        if (_hit.collider.TryGetComponent<Target>(out Target targetComponent) && targetComponent.faction != unit.faction && _hit.collider.gameObject != unit.gameObject)
        {
            unit.set_nav_agent_destination_and_target(_hit.collider.transform.position, targetComponent.gameObject);
        }
        else
        {
            unit.agent.SetDestination(_hit.point);
            unit.agent.stoppingDistance = 0f;
            unit.target = null;
        }

    }
}
