using System;
using System.Collections.Generic;
using UnityEngine;

public class BeliefFactory
{
    readonly GoapAgent agent;
    readonly Dictionary<AgentBelief.BeliefType, AgentBelief> beliefs;

    public BeliefFactory(GoapAgent agent, Dictionary<AgentBelief.BeliefType, AgentBelief> beliefs)
    {
        this.agent = agent;
        this.beliefs = beliefs;
    }

    public void AddBelief(AgentBelief.BeliefType key, Func<bool> condition)
    {
        beliefs.Add(key, new AgentBelief.Builder(key).WithCondition(condition).Build());
    }

    public void AddSensorBelief(AgentBelief.BeliefType key, Sensor sensor)
    {
        beliefs.Add(key, new AgentBelief.Builder(key).WithCondition(() => sensor.IsTargetInRange).WithLocation(() => sensor.TargetPosition).Build());
    }
    public void AddLocationBelief(AgentBelief.BeliefType key, float distance, Transform locationCondition)
    {
       AddLocationBelief(key, distance, locationCondition.position);
    }

    public void AddLocationBelief(AgentBelief.BeliefType key, float distance ,Vector3 locationCondition)
    {
        beliefs.Add(key, new AgentBelief.Builder(key).WithCondition(() => InRangeOf(locationCondition, distance)).WithLocation(() => locationCondition).Build());
    }

    bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(agent.transform.position, pos) <= range;   

}

public class AgentBelief
{
    Func<bool> condition = () => false;
    Func<Vector3> observedLocation = () => Vector3.zero;

    public Vector3 Location => observedLocation();

    BeliefType beliefType;

    public enum BeliefType
    {
        Nothing,
        HasAvailableWorkers,
        HasAvailableTroops,
        TroopsSelected,
        WorkersSelected,
        WoodResourceAvailable,
        StoneResourceAvailable,
        IronResourceAvailable,
        ResourcesAreSufficient,
        workersAssignedToWood,
        workersAssignedToStone,
        workersAssignedToIron,
        WoodIncomeIsIncreasing,
        CanAffordWoodWorkplace,
        HasWoodWorkplace,
        CanAffordStoneWorkplace,
        HasStoneWorkplace,
        CanAffordIronWorkplace,
        HasIronWorkplace,
        CanAffordDesiredWorkersAmount,
        CanAffordDesiredTroopsAmount,
        HasEnoughWorkers,
        // Military
        CanAffordBlacksmith,
        HasBlacksmith,
        CanAffordBarracks,
        HasBarracks,
        HasEnoughTroops,
        troopsAssignedToTarget,
        TargetsAvailable,
    }

    AgentBelief(BeliefType beliefType) 
    {
 
        this.beliefType = beliefType;
    }

    public bool Evaluate()
    { 
        return condition(); 
    }

    public class Builder
    {
        readonly AgentBelief belief;

        public Builder( BeliefType beliefType)
        {
            belief = new AgentBelief( beliefType);
        }

        public Builder WithCondition(Func<bool> condition)
        {
            belief.condition = condition;
            return this;
        }

        public Builder WithLocation(Func<Vector3> observedLocation)
        {
            belief.observedLocation = observedLocation;
            return this;
        }

        public AgentBelief Build()
        {
            return belief;
        }
    }
 
}
