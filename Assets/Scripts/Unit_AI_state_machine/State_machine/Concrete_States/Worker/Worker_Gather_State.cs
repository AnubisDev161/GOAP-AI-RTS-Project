using UnityEngine;
using Unity.Collections;
using Unity.VisualScripting;
using Unity.Mathematics;
using System.Resources;
public class Worker_Gather_State : Worker_State
{
    protected float attack_time = 4f;
    protected float current_attack_time = 0f;

    public Worker_Gather_State(Worker worker, Worker_StateMachine worker_stateMachine) : base(worker, worker_stateMachine)
    {
    }

    public override void animation_trigger_event(Worker.animation_trigger_type trigger_Type)
    {
        base.animation_trigger_event(trigger_Type);
        if (trigger_Type == Worker.animation_trigger_type.worker_started_gathering)
        {
            TryGather();
        }
    }

    public override void enter_state()
    {
        base.enter_state();
        current_attack_time = 0f;
        worker.agent.isStopped = false;
        worker.animator.SetTrigger("Attack");
    }

    public override void exit_state()
    {
        base.exit_state();
        current_attack_time = 0f;
        worker.enable_selection_material(false);
        worker.agent.isStopped = false;
    }

    public override void frame_update()
    {
        base.frame_update();
        if (!check_mouse_input() && worker.target != null)
        {
            if (Mathf.Abs((worker.target.transform.position - worker.transform.position).magnitude) > worker.min_gather_distance)
            {
                worker.stateMachine.change_state(worker.worker_going_to_resource);
                return;
            }

        }
        else if (worker.target == null)
        {
            if (worker.targets_in_range.Count > 0)
            {
                worker.find_closest_target_in_range();
                return;
            }
            else
            {
                worker.stateMachine.change_state(worker.worker_going_to_worklpace);
                return;
            }

        }
        current_attack_time += Time.deltaTime;
        if (current_attack_time >= attack_time)
        {
                TryGather(); 
        }
        else
        {
            worker.meshRenderer.material.color = Color.Lerp(worker.meshRenderer.material.color, Color.black, Time.deltaTime);
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

        var resource_component = _hit.collider.GetComponent<IClickable>();
        if (resource_component != null  && _hit.collider.gameObject != worker.gameObject)
        {
            worker.set_nav_agent_destination_and_target(resource_component.get_parent());
        }
        else
        {
            worker.agent.SetDestination(_hit.point);
            worker.agent.stoppingDistance = 0f;
            worker.target = null;
            worker.stateMachine.change_state(worker.worker_going_to_mouse_click_position);
        }
        worker.order_placed = true;
       
    }
    public virtual void TryGather()
    {
        current_attack_time = 0;
        worker.enable_selection_material(false);
        if (worker.target.TryGetComponent<Resource>(out Resource component))
        {
            if (worker.gatheredResourcesPack + component.TryCollect() <= worker.maxGatherCapacity)
            {
                worker.gatheredResourcesPack = worker.gatheredResourcesPack + component.Collect();
            }
            else
            {
                worker.stateMachine.change_state(worker.worker_going_to_worklpace);
            }
        }

    }

}

