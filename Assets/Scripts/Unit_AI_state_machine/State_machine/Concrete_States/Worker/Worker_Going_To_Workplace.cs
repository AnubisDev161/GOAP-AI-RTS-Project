using UnityEngine;

public class Worker_Going_To_Worklpace : Worker_State
{
    public Worker_Going_To_Worklpace(Worker worker, Worker_StateMachine worker_stateMachine) : base(worker, worker_stateMachine)
    {
    }

    public override void animation_trigger_event(Worker.animation_trigger_type trigger_Type)
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
        if (check_mouse_input()) { }
        else if (worker.target != null && worker.target != worker.workplace && !worker.order_placed)
        {
            worker.stateMachine.change_state(worker.worker_going_to_resource);
        }
        else if (worker.workplace != null )
        {

            if (Mathf.Abs((worker.workplace.transform.position - worker.transform.position).magnitude) > worker.max_distance_to_workplace)
            {
                return_to_workplace();

            }else
            {
                worker.stateMachine.change_state(worker.worker_offload_state);
            }
        }
        else
        {
            worker.stateMachine.change_state(worker.worker_idle_state);
        }
    }

    public override void handle_mouse_input(Ray _ray, RaycastHit _hit)
    {
        var resource_component = _hit.collider.GetComponent<Resource>();
        if (resource_component != null && _hit.collider.gameObject != worker.gameObject)
        {
            worker.set_nav_agent_destination_and_target(_hit.collider.gameObject);
            worker.stateMachine.change_state(worker.worker_going_to_resource);
        }
        else
        {
            worker.agent.SetDestination(_hit.point);
            worker.agent.stoppingDistance = 0f;
            worker.stateMachine.change_state(worker.worker_idle_state);
        }
        worker.order_placed = true;
        //if (!worker.hold_position)
        //{
        //    worker.timer.StartTimer();
        //}
    }
    public override bool check_mouse_input()
    {
        return base.check_mouse_input();
    }
    public override void physics_update()
    {
        base.physics_update();
    }

    public void return_to_workplace()
    {
        worker.agent.SetDestination(worker.workplace.transform.position);
        worker.agent.stoppingDistance = worker.max_distance_to_workplace;
      
    }
}