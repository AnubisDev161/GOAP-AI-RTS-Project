using System;
using UnityEngine;

public class Unit_StateMachine
{
    public Unit_State current_unit_state {  get; set; }
    public void initialize(Unit_State starting_state)
    {
        current_unit_state = starting_state;
     //   current_unit_state.unit.state_label.text = ("current state:" + current_unit_state.ToString() + Environment.NewLine + current_unit_state.unit.faction.ToString());
    }

    public void change_state(Unit_State new_state)
    {
        current_unit_state.exit_state();
        current_unit_state = new_state;
        current_unit_state.enter_state();
        if (current_unit_state != null || current_unit_state is not Unit_Die_State)
        {
           
           // current_unit_state.unit.state_label.text = ("current state:" + current_unit_state.ToString() + Environment.NewLine + current_unit_state.unit.faction.ToString());
        }
    }
}
