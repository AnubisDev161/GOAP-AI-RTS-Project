using System;
using UnityEngine;

public class Worker_StateMachine
{

  public Worker_State current_worker_state { get; set; }
    public void initialize(Worker_State starting_state)
    {
        current_worker_state = starting_state;
        current_worker_state.worker.state_label.text = ("current state:" + current_worker_state.ToString() + Environment.NewLine + current_worker_state.worker.factionType.ToString());
    }

    public void change_state(Worker_State new_state)
    {
        current_worker_state.exit_state();
        current_worker_state = new_state;
        current_worker_state.enter_state();
        if (current_worker_state != null || current_worker_state is not Worker_Die_State)
        {
            current_worker_state.worker.state_label.text = ("current state:" + current_worker_state.ToString() + Environment.NewLine + current_worker_state.worker.factionType.ToString());
        }
    }
}
