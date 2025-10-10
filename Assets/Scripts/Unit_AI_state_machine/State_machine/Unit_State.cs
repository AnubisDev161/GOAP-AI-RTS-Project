using System.Linq;
using UnityEngine;
using Unity.Mathematics;

public class Unit_State
{
    public Unit unit;
    protected Unit_StateMachine unit_stateMachine;
   
    public Unit_State(Unit unit, Unit_StateMachine unit_stateMachine)
    {
        this.unit = unit;
        this.unit_stateMachine = unit_stateMachine;
    }
   
    public virtual void enter_state() { }
    public virtual void exit_state() { }
    public virtual void frame_update() { }
    public virtual void physics_update() {  }
    public virtual bool check_mouse_input() {
        bool mouse_clicked = false;
        if (unit.follow_mouse_click_position && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = unit.player_cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unit.ground))
            {
                mouse_clicked = true;
                handle_mouse_input(ray, hit);
            }
        }
        return mouse_clicked;
    }
    public virtual void handle_mouse_input(Ray _ray, RaycastHit _hit) { }
    
    public virtual void animation_trigger_event(Unit.animation_trigger_type trigger_Type) { }
}
