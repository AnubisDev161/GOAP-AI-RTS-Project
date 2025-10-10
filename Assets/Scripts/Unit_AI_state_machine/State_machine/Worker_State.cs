using System.Linq;
using UnityEngine;
using Unity.Mathematics;
public class Worker_State
{
  
    public Worker worker;
    protected Worker_StateMachine worker_stateMachine;

    public Worker_State(Worker worker, Worker_StateMachine worker_stateMachine)
    {
        this.worker = worker;
        this.worker_stateMachine = worker_stateMachine;
    }

    public virtual void enter_state() { }
    public virtual void exit_state() { }
    public virtual void frame_update() { }
    public virtual void physics_update() { }
    public virtual bool check_mouse_input()
    {
        bool mouse_clicked = false;
        if (worker.follow_mouse_click_position && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = worker.player_cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, worker.ground))
            {
                mouse_clicked = true;
                handle_mouse_input(ray, hit);
            }
        }
        return mouse_clicked;
    }
    public virtual void handle_mouse_input(Ray _ray, RaycastHit _hit) { }

    public virtual void animation_trigger_event(Worker.animation_trigger_type trigger_Type) { }
}
