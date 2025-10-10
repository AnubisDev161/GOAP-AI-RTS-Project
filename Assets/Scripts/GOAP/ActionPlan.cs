using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ActionPlan
{
    public AgentGoal agentGoal {  get; }
    public Stack<AgentAction> actions { get; }
    public float totalCost { get; set; }

    public ActionPlan(AgentGoal goal, Stack<AgentAction> actions, float totalcost)
    {
        this.agentGoal = goal;
        this.actions = actions;
        this.totalCost = totalcost;
    }
}

