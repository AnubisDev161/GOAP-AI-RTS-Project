using NUnit.Framework;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Worker_Going_To_Resource : Worker_State
{
    public Worker_Going_To_Resource(Worker worker, Worker_StateMachine worker_stateMachine) : base(worker, worker_stateMachine)
    {
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
        if (!check_mouse_input() && worker.target != null)
        {
            if(!worker.HasAppropriateWorkplace() &&!worker.TryFindAppropriateWorkplace())
            {
                worker.target = null;
                worker.stateMachine.change_state(worker.worker_going_to_mouse_click_position);

                return;
            }
            if (Mathf.Abs((worker.target.transform.position - worker.transform.position).magnitude) > worker.min_gather_distance)
            {
                worker.set_nav_agent_destination_and_target(worker.target);
            }
            else
            {
                worker.stateMachine.change_state(worker.worker_gather_state);
               
            }

        }
        else if (worker.target == null)
        {
            if (worker.targets_in_range.Count > 0)
            {
                worker.find_closest_target_in_range();
            }
            else
            {
                worker.stateMachine.change_state(worker.worker_going_to_worklpace);
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
            worker.stateMachine.change_state(worker.worker_going_to_mouse_click_position);

        }
        worker.order_placed = true;
       
        //if (!worker.hold_position)
        //{
        //    worker.timer.StartTimer();
        //}

    }
}
