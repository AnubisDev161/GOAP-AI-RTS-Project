using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AgentAction
{
    //public string name { get; }
    public float cost { get; private set; }

    public HashSet<AgentBelief> preconditions { get; } = new();
    public HashSet<AgentBelief> effects { get; } = new();

    IActionStrategy strategy;
    public bool complete => strategy.complete;

    public ActionType actionType { get; private set; }

    public enum ActionType
    {
        Idle,
        GatherWood,
        SelectWorkers,
        FindResource,
        AssignWorkersToWoodResource,
        AssignWorkersToStoneResource,
        AssignWorkersToIronResource,
        ConstructWoodWorkplace,
        ConstructStoneWorkplace,
        ConstructIronWorkplace,
        ConstructBlacksmith,
        ConstructBarracks,
        RecruitWorkers,
        RecruitTroops,
        SelectTroops,
        AssignTroopsToTargets,
        FindTarget,
        AssignAllWaitingWorkers


    }
    AgentAction(ActionType actionType)
    {
        this.actionType = actionType;
    }

    public void Start() => strategy.Start();

    public void Update(float deltaTime)
    {
        if (strategy.canPerform)
        {
            strategy.Update(deltaTime);
        }

        // Bail out if the strategy is still executing 
        if (!strategy.complete) return;

        // Applay effects
        foreach (var effect in effects)
        {
            effect.Evaluate();
        }
    }
    public void Stop() => strategy.Stop();

    public class Builder
    {
        readonly AgentAction action;

        public Builder( ActionType actionType)
        {
            action = new AgentAction( actionType)
            {
                cost = 1
            };
        }

        public Builder WithCost(float cost)
        {
            action.cost = cost;
            return this;
        }

        public Builder WithStrategy(IActionStrategy strategy)
        {
            action.strategy = strategy;
            return this;
        }

        public Builder AddPrecondition(AgentBelief precondition)
        {
            action.preconditions.Add(precondition);
            return this;
        }

        public Builder AddEffect(AgentBelief effect)
        {
            action.effects.Add(effect);
            return this;
        }

        public AgentAction Build()
        {
            return action;
        }
    }
}



