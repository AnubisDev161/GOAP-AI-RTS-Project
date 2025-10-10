using UnityEngine;

public class Unit_Idle_State : Unit_State
{
    public Unit_Idle_State(Unit unit, Unit_StateMachine unit_stateMachine) : base(unit, unit_stateMachine)
    {
    }
    public override void animation_trigger_event(Unit.animation_trigger_type trigger_Type)
    {
        base.animation_trigger_event(trigger_Type);
    }

    public override void enter_state()
    {
        base.enter_state();

        if (unit.factionType == Target.factionTypes.player) return;

        var newTarget = unit.faction.FindTarget();

        if (newTarget != null)
        {
            unit.set_nav_agent_destination_and_target(newTarget.transform.position, newTarget);
        }
    }

    public override void frame_update()
    {
        base.frame_update();
        if (check_mouse_input()) { }
        else if (unit.target != null && unit.target != unit.commando_unit)
        {
            unit.stateMachine.change_state(unit.unit_chase_state);
        }
    }

    public override void handle_mouse_input(Ray _ray, RaycastHit _hit)
    {
        var potential_target = _hit.collider.GetComponent<Target>();
        if (potential_target != null && potential_target.faction != unit.faction && _hit.collider.gameObject != unit.gameObject)
        {
            unit.set_nav_agent_destination_and_target(_hit.collider.transform.position, potential_target.gameObject);
            unit.stateMachine.change_state(unit.unit_chase_state);
        }
        else
        {
            unit.agent.SetDestination(_hit.point);
            unit.agent.stoppingDistance = 0f;
            unit.stateMachine.change_state(unit.unit_walk_state);
        }
    }

    public override void physics_update()
    {
        base.physics_update();
    }
}
