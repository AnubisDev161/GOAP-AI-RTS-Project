using UnityEngine;

public class Worker_Die_State : Worker_State
{
    public Worker_Die_State(Worker worker, Worker_StateMachine worker_stateMachine) : base(worker, worker_stateMachine)
    {

    }

    public override void animation_trigger_event(Worker.animation_trigger_type trigger_Type)
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
       worker.dead = true;
       worker.follow_mouse_click_position = false;

       worker.GetComponentInChildren<Worker_View_Range>().enabled = false;
       worker.GetComponentInChildren<SphereCollider>().enabled = false;
       worker.die();

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
