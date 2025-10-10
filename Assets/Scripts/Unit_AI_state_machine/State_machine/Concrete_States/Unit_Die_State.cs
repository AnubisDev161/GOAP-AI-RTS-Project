using UnityEngine;

public class Unit_Die_State : Unit_State
{
    public Unit_Die_State(Unit unit, Unit_StateMachine unit_stateMachine) : base(unit, unit_stateMachine)
    {

    }

    public override void animation_trigger_event(Unit.animation_trigger_type trigger_Type)
    {
        base.animation_trigger_event(trigger_Type);
    }

    public override bool check_mouse_input()
    {
        return base.check_mouse_input();
    }

    public override void enter_state()
    {
        base.enter_state();
        unit.follow_mouse_click_position = false;
        unit.dead = true;
        unit.GetComponentInChildren<Unit_Opponent_In_View_Range_Check>().enabled = false;
        unit.GetComponentInChildren<SphereCollider>().enabled = false;
        unit.die();
        
    }

    public override void exit_state()
    {
        base.exit_state();
    }

    public override void frame_update()
    {
        base.frame_update();
    }

    public override void handle_mouse_input(Ray _ray, RaycastHit _hit)
    {
        base.handle_mouse_input(_ray, _hit);
    }

    public override void physics_update()
    {
        base.physics_update();
    }
}
