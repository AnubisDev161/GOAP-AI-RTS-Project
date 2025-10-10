using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;

public class AgentGoal
{
    public float priority { get; private set; }
    public HashSet<AgentBelief> desiredEffects { get; } = new();

    AgentGoal(GoalType goalType)
    {
        this.goalType = goalType;
    }

    public GoalType goalType { get; private set; }

    public enum GoalType
    {
        Idle,
        MaintainResourceSurplus,
        SufficientResourceProduction,
        CollectWood,
        CollectStone,
        CollectIron,
        ConstructWoodWorkplace,
        ConstructStoneWorkplace,
        ConstructIronWorkplace,
        ConstructBlacksmith,
        ConstructBarracks,
        RecruitWorkers,
        RecruitTroops,
        AttackOpponent,
        HaveEnoughWorkers,
        AssignWaitingWorkers,
    }

    public class Builder 
    {
        readonly AgentGoal goal;
        public Builder(GoalType goalType)
        {
            goal = new AgentGoal(goalType);
           
        }

        public Builder WithPriority(float priority)
        {
            goal.priority = priority;
            return this;
        }

        public Builder WithDesiredEffect(AgentBelief effect)
        {
            goal.desiredEffects.Add(effect);
            return this;
        }

        public AgentGoal Build()
        {
            return goal;
        }

    }

}

