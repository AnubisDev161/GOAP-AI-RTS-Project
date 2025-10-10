using UnityEngine;
using System.Collections.Generic;
public class Unit_Long_Range_Attack_State : Unit_Attack_State
{
    public Unit_Long_Range_Attack_State(Long_Range_Unit unit, Unit_StateMachine unit_stateMachine) : base(unit, unit_stateMachine)
    {

    }
    
    public override void attack()
    {
        unit.GetComponent<Long_Range_Unit>().long_range_attack();
        current_attack_time = 0;
        unit.enable_selection_material(false);

    }


}
