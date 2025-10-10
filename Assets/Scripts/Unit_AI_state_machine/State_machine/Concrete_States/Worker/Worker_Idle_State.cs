using System;
using UnityEngine;

public class Worker_Idle_State : Worker_State
{
    public Worker_Idle_State(Worker worker, Worker_StateMachine worker_stateMachine) : base(worker, worker_stateMachine)
    {

    }

    public override void animation_trigger_event(Worker.animation_trigger_type trigger_Type)
    {
        base.animation_trigger_event(trigger_Type);
    }

    public override void enter_state()
    {
        if (worker.target == null && worker.workplace != null && worker.factionType != Target.factionTypes.player)
        {
            worker.target = worker.workplace.RequestNewTargetResource();
            if (worker.target == null)
            {
                worker.faction.RemoveWorkerFromBusyList(worker);
            }
        }
    } 

    public override void exit_state()
    {
        base.exit_state();
    }

    public override void frame_update()
    {
        base.frame_update();
        if (check_mouse_input()) { }
        else if(worker.target != null)
        {
            worker.stateMachine.change_state(worker.worker_going_to_resource);
        }
       
    }
    public override void handle_mouse_input(Ray _ray, RaycastHit _hit)
    {
        var unit_component = _hit.collider.GetComponent<Resource>();
        if (unit_component != null && _hit.collider.gameObject != worker.gameObject)
        {
            worker.set_nav_agent_destination_and_target( _hit.collider.gameObject);
            worker.stateMachine.change_state(worker.worker_going_to_resource);
        }
        else
        {
       
            worker.agent.SetDestination(_hit.point);
            worker.agent.stoppingDistance = 0f;
            worker.stateMachine.change_state(worker.worker_going_to_mouse_click_position);
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
}
