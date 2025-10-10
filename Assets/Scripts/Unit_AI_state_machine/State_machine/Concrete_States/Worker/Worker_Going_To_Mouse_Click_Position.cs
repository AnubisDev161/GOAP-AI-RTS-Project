using UnityEngine;

public class Worker_Going_To_Mouse_Click_Position : Worker_State
{
    public Worker_Going_To_Mouse_Click_Position(Worker worker, Worker_StateMachine worker_stateMachine) : base(worker, worker_stateMachine)
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
        worker.target = null;
    }

    public override void exit_state()
    {
        base.exit_state();
    }

    public override void frame_update()
    {
        base.frame_update();

        if (!check_mouse_input() )
        {
            
            if (worker.agent.remainingDistance == 0)
            {
                
                worker.stateMachine.change_state(worker.worker_idle_state);
            }

        }
      
    }
    public override void handle_mouse_input(Ray _ray, RaycastHit _hit)
    {
        var target_Resource_component = _hit.collider.GetComponent<Resource>();
        if (target_Resource_component != null && _hit.collider.gameObject != worker.gameObject)
        {
            worker.set_nav_agent_destination_and_target(target_Resource_component.gameObject);
        }
        else
        {
            worker.agent.SetDestination(_hit.point);
            worker.agent.stoppingDistance = 0f;
            worker.target = null;
        }
        worker.order_placed = true;
    }

    public override void physics_update()
    {
        base.physics_update();
    }
}
