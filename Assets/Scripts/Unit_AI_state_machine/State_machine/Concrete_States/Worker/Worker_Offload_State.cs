using UnityEngine;

public class Worker_Offload_State : Worker_State
{
    float timer;
    public Worker_Offload_State(Worker worker, Worker_StateMachine worker_stateMachine) : base(worker, worker_stateMachine)
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
        worker.offload();
    
        if (worker.target == null && worker.workplace != null)
        {
            worker.target = worker.workplace.RequestNewTargetResource();
            if(worker.target == null)
            {
                worker.faction.RemoveWorkerFromBusyList(worker);
            }
        }
        worker.agent.isStopped = true;
        timer = 0f;
    }

    public override void exit_state()
    {
        base.exit_state();
        worker.agent.isStopped = false;
    }

    public override void frame_update()
    {
        base.frame_update();

        timer += Time.deltaTime;

        if (timer > worker.offload_time)
        {
            worker.stateMachine.change_state(worker.worker_idle_state);
        }
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
