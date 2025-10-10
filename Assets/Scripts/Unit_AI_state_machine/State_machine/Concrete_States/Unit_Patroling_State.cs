using Unity.VisualScripting;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using Unity.VisualScripting.FullSerializer;

public class Unit_Patroling_State : Unit_State
{
    private Vector3 initial_position = Vector3.zero;
    public Unit_Patroling_State(Unit unit, Unit_StateMachine unit_stateMachine) : base(unit, unit_stateMachine)
    {
        initial_position = unit.transform.position;
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
        unit.agent.stoppingDistance = 1f;
        unit.agent.speed = unit.patroling_speed;
       
    }

    public override void exit_state()
    {
        base.exit_state();
    }

    public override void frame_update()
    {
        base.frame_update();
        
        if (unit.target != null && unit.target != unit.commando_unit)
        {
            unit.stateMachine.change_state(unit.unit_chase_state);
        }
        else if (unit.agent.remainingDistance <= unit.agent.stoppingDistance) {
            Vector3 point;
            if (find_random_patrol_point(initial_position, unit.max_patroling_distance, out point)) {

                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                unit.agent.SetDestination(point);
            }
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
    private bool find_random_patrol_point(Vector3 center, float range, out Vector3 result)
    {
        Vector3 random_point = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(random_point, out hit, 1.0f, NavMesh.AllAreas)){
           result = hit.position;
           return true;
        }
        result = Vector3.zero;
        return false;
    }
}
