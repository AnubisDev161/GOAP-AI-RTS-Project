using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(IGoapInteractor))]
public class GoapAgent : MonoBehaviour
{
    [SerializeField] int desiredWorkersCount = 5;
    [SerializeField] int desiredTroopsCount = 5;

    IGoapInteractor goapInteractor;
    IGoapPlanner goapPlanner;

    AgentGoal lastGoal;
    public AgentGoal currentGoal;
    public ActionPlan actionPlan;
    public AgentAction currentAction;

    public Dictionary<AgentBelief.BeliefType, AgentBelief> beliefs;
    public HashSet<AgentAction> actions;
    public HashSet<AgentGoal> goals;

    [SerializeField] int woodWorkerCount;
    [SerializeField] int stoneWorkerCount;
    [SerializeField] int ironWorkerCount;

    private void Awake()
    {
        goapPlanner = new GoapPlanner();
        goapInteractor = GetComponent<IGoapInteractor>();
    }

    private void Start()
    {
        SetupBeliefs();
        SetupActions();
        SetupGoals();
    }

    // Beliefs tell the agent about the current world state 
    // with beliefs the agent keeps track of whether it has satisfied goal or not
    private void SetupBeliefs()
    {
        beliefs = new Dictionary<AgentBelief.BeliefType, AgentBelief>();
        BeliefFactory factory = new BeliefFactory(this, beliefs);

        factory.AddBelief(AgentBelief.BeliefType.Nothing, () => false);

        // resources
        {
            factory.AddBelief(AgentBelief.BeliefType.WoodResourceAvailable, () => goapInteractor.GetSearchedResource(Resource.ResourceTypes.Wood) != null);
            factory.AddBelief(AgentBelief.BeliefType.StoneResourceAvailable, () => goapInteractor.GetSearchedResource(Resource.ResourceTypes.Stone) != null);
            factory.AddBelief(AgentBelief.BeliefType.IronResourceAvailable, () => goapInteractor.GetSearchedResource(Resource.ResourceTypes.Iron) != null);
        }

        // workplaces
        {
            factory.AddBelief(AgentBelief.BeliefType.CanAffordWoodWorkplace, () => goapInteractor.HasFactionEnoughResources(goapInteractor.GetBuildingCost(Building.BuildingTypes.WoodWorkplace)));
            factory.AddBelief(AgentBelief.BeliefType.HasWoodWorkplace, () => goapInteractor.HasBuildings(Building.BuildingTypes.WoodWorkplace) > 0);
            factory.AddBelief(AgentBelief.BeliefType.CanAffordStoneWorkplace, () => goapInteractor.HasFactionEnoughResources(goapInteractor.GetBuildingCost(Building.BuildingTypes.StoneWorkplace)));
            factory.AddBelief(AgentBelief.BeliefType.HasStoneWorkplace, () => goapInteractor.HasBuildings(Building.BuildingTypes.StoneWorkplace) > 0);
            factory.AddBelief(AgentBelief.BeliefType.CanAffordIronWorkplace, () => goapInteractor.HasFactionEnoughResources(goapInteractor.GetBuildingCost(Building.BuildingTypes.IronWorkplace)));
            factory.AddBelief(AgentBelief.BeliefType.HasIronWorkplace, () => goapInteractor.HasBuildings(Building.BuildingTypes.IronWorkplace) > 0);
        }

        // other buildings
        {
            factory.AddBelief(AgentBelief.BeliefType.CanAffordBlacksmith, () => goapInteractor.HasFactionEnoughResources(goapInteractor.GetBuildingCost(Building.BuildingTypes.Blacksmith)));
            factory.AddBelief(AgentBelief.BeliefType.HasBlacksmith, () => goapInteractor.HasBuildings(Building.BuildingTypes.Blacksmith) > 0);

            factory.AddBelief(AgentBelief.BeliefType.CanAffordBarracks, () => goapInteractor.HasFactionEnoughResources(goapInteractor.GetBuildingCost(Building.BuildingTypes.Barracks)));
            factory.AddBelief(AgentBelief.BeliefType.HasBarracks, () => goapInteractor.HasBuildings(Building.BuildingTypes.Barracks) > 0);
        }

        // Workers
        {
            factory.AddBelief(AgentBelief.BeliefType.CanAffordDesiredWorkersAmount, () => goapInteractor.CanFactionAffordWorker(Target.factionTypes.CPU));
            factory.AddBelief(AgentBelief.BeliefType.CanAffordDesiredTroopsAmount, () => goapInteractor.CanFactionAffordTroops(desiredTroopsCount));

            factory.AddBelief(AgentBelief.BeliefType.HasEnoughWorkers, () => goapInteractor.GetAllWorkersCount() >= desiredWorkersCount);
            factory.AddBelief(AgentBelief.BeliefType.HasAvailableWorkers, () => goapInteractor.HasAvailableWorkers() > 0);
            factory.AddBelief(AgentBelief.BeliefType.WorkersSelected, () => goapInteractor.HasSelectedWorkers());

            factory.AddBelief(AgentBelief.BeliefType.workersAssignedToWood, () => goapInteractor.GetWorkersAssignedToResourceCount(Resource.ResourceTypes.Wood) >= woodWorkerCount);
            factory.AddBelief(AgentBelief.BeliefType.workersAssignedToStone, () => goapInteractor.GetWorkersAssignedToResourceCount(Resource.ResourceTypes.Stone) >= stoneWorkerCount);
            factory.AddBelief(AgentBelief.BeliefType.workersAssignedToIron, () => goapInteractor.GetWorkersAssignedToResourceCount(Resource.ResourceTypes.Iron) >= ironWorkerCount);
        }

        // troops 
        {
            factory.AddBelief(AgentBelief.BeliefType.HasEnoughTroops, () => goapInteractor.HasAvailableTroops() >= desiredTroopsCount);

            // for example does the agent know that it has troops if the return value of goapInteractor.HasAvailableTroops() is grater than 0
            factory.AddBelief(AgentBelief.BeliefType.HasAvailableTroops, () => goapInteractor.HasAvailableTroops() > 0);
            factory.AddBelief(AgentBelief.BeliefType.TroopsSelected, () => goapInteractor.HasSelectedTroops() == true);
            factory.AddBelief(AgentBelief.BeliefType.TargetsAvailable, () => goapInteractor.GetAllAvailableTargets().Count() > 0);
            factory.AddBelief(AgentBelief.BeliefType.troopsAssignedToTarget, () => goapInteractor.GetTroopsAssignedTargetsCount(Faction.factionTypes.Player) >= goapInteractor.GetAllAvailableTargets().Count());
        }
    }

    // An action plan consists of one or more actions which need to be executed to satisfy a goal
    private void SetupActions()
    {
        actions = new HashSet<AgentAction>();

        actions.Add(new AgentAction.Builder(AgentAction.ActionType.Idle).WithStrategy(new IdleStrategy(5)).AddEffect(beliefs[AgentBelief.BeliefType.Nothing]).Build());
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.SelectWorkers).WithStrategy(new SelectWorkers(goapInteractor)).AddPrecondition(beliefs[AgentBelief.BeliefType.HasAvailableWorkers]).AddEffect(beliefs[AgentBelief.BeliefType.WorkersSelected]).Build());
     
        // find resource actions
        
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.FindResource).WithStrategy(new FindResource(goapInteractor, Resource.ResourceTypes.Wood)).AddPrecondition(beliefs[AgentBelief.BeliefType.WorkersSelected]).AddEffect(beliefs[AgentBelief.BeliefType.WoodResourceAvailable]).Build());
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.FindResource).WithStrategy(new FindResource(goapInteractor, Resource.ResourceTypes.Stone)).AddPrecondition(beliefs[AgentBelief.BeliefType.WorkersSelected]).AddEffect(beliefs[AgentBelief.BeliefType.StoneResourceAvailable]).Build());
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.FindResource).WithStrategy(new FindResource(goapInteractor, Resource.ResourceTypes.Iron)).AddPrecondition(beliefs[AgentBelief.BeliefType.WorkersSelected]).AddEffect(beliefs[AgentBelief.BeliefType.IronResourceAvailable]).Build());
       
        // assign workers to resource action setup

        actions.Add(new AgentAction.Builder(AgentAction.ActionType.AssignWorkersToWoodResource).WithStrategy(new AssignWorkersToResource(goapInteractor, Resource.ResourceTypes.Wood)).
            AddPrecondition(beliefs[AgentBelief.BeliefType.WoodResourceAvailable]).AddPrecondition((beliefs[AgentBelief.BeliefType.WorkersSelected])).AddPrecondition(beliefs[AgentBelief.BeliefType.HasWoodWorkplace]).
            AddEffect(beliefs[AgentBelief.BeliefType.workersAssignedToWood]).Build());

        actions.Add(new AgentAction.Builder(AgentAction.ActionType.AssignWorkersToStoneResource).WithStrategy(new AssignWorkersToResource(goapInteractor, Resource.ResourceTypes.Stone)).
           AddPrecondition(beliefs[AgentBelief.BeliefType.StoneResourceAvailable]).AddPrecondition((beliefs[AgentBelief.BeliefType.WorkersSelected])).AddPrecondition(beliefs[AgentBelief.BeliefType.HasStoneWorkplace]).
           AddEffect(beliefs[AgentBelief.BeliefType.workersAssignedToStone]).Build());

        actions.Add(new AgentAction.Builder(AgentAction.ActionType.AssignWorkersToIronResource).WithStrategy(new AssignWorkersToResource(goapInteractor, Resource.ResourceTypes.Iron)).
           AddPrecondition(beliefs[AgentBelief.BeliefType.IronResourceAvailable]).AddPrecondition((beliefs[AgentBelief.BeliefType.WorkersSelected])).AddPrecondition(beliefs[AgentBelief.BeliefType.HasIronWorkplace]).
           AddEffect(beliefs[AgentBelief.BeliefType.workersAssignedToIron]).Build());

        // construction of worklpaces

        actions.Add(new AgentAction.Builder(AgentAction.ActionType.ConstructWoodWorkplace).WithStrategy(new ConstructBuilding(goapInteractor, Building.BuildingTypes.WoodWorkplace)).AddPrecondition(beliefs[AgentBelief.BeliefType.CanAffordWoodWorkplace]).AddEffect(beliefs[AgentBelief.BeliefType.HasWoodWorkplace]).Build());
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.ConstructStoneWorkplace).WithStrategy(new ConstructBuilding(goapInteractor, Building.BuildingTypes.StoneWorkplace)).AddPrecondition(beliefs[AgentBelief.BeliefType.CanAffordStoneWorkplace]).AddEffect(beliefs[AgentBelief.BeliefType.HasStoneWorkplace]).Build());
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.ConstructIronWorkplace).WithStrategy(new ConstructBuilding(goapInteractor, Building.BuildingTypes.IronWorkplace)).AddPrecondition(beliefs[AgentBelief.BeliefType.CanAffordIronWorkplace]).AddEffect(beliefs[AgentBelief.BeliefType.HasIronWorkplace]).Build());

        // other buildings
        
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.ConstructBlacksmith).WithStrategy(new ConstructBuilding(goapInteractor, Building.BuildingTypes.Blacksmith)).AddPrecondition(beliefs[AgentBelief.BeliefType.CanAffordBlacksmith]).AddEffect(beliefs[AgentBelief.BeliefType.HasBlacksmith]).Build());
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.ConstructBarracks).WithStrategy(new ConstructBuilding(goapInteractor, Building.BuildingTypes.Barracks)).AddPrecondition(beliefs[AgentBelief.BeliefType.CanAffordBarracks]).AddEffect(beliefs[AgentBelief.BeliefType.HasBarracks]).Build());

        // recruiting workers

        actions.Add(new AgentAction.Builder(AgentAction.ActionType.RecruitWorkers).WithStrategy(new RecruitWorkers(goapInteractor, desiredWorkersCount)).AddPrecondition(beliefs[AgentBelief.BeliefType.CanAffordDesiredWorkersAmount]).AddEffect(beliefs[AgentBelief.BeliefType.HasEnoughWorkers]).AddEffect(beliefs[AgentBelief.BeliefType.HasAvailableWorkers]).Build());

        // recruiting troops & target selection

        actions.Add(new AgentAction.Builder(AgentAction.ActionType.RecruitTroops).WithStrategy(new RecruitTroops(goapInteractor, desiredTroopsCount)).AddPrecondition(beliefs[AgentBelief.BeliefType.CanAffordDesiredTroopsAmount]).AddEffect(beliefs[AgentBelief.BeliefType.HasEnoughTroops]).AddEffect(beliefs[AgentBelief.BeliefType.HasAvailableTroops]).Build());
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.SelectTroops).WithStrategy(new SelectTroops(goapInteractor)).AddPrecondition(beliefs[AgentBelief.BeliefType.HasAvailableTroops]).AddEffect(beliefs[AgentBelief.BeliefType.TroopsSelected]).Build());
        actions.Add(new AgentAction.Builder(AgentAction.ActionType.FindTarget).WithStrategy(new FindTarget(goapInteractor)).AddPrecondition(beliefs[AgentBelief.BeliefType.HasAvailableTroops]).AddEffect(beliefs[AgentBelief.BeliefType.TargetsAvailable]).Build());

        actions.Add(new AgentAction.Builder(AgentAction.ActionType.AssignTroopsToTargets).WithStrategy(new AssignTroopsToTargets(goapInteractor, Faction.factionTypes.Player)).
          AddPrecondition(beliefs[AgentBelief.BeliefType.TargetsAvailable]).AddPrecondition((beliefs[AgentBelief.BeliefType.TroopsSelected])).AddPrecondition(beliefs[AgentBelief.BeliefType.HasEnoughTroops]).
          AddEffect(beliefs[AgentBelief.BeliefType.troopsAssignedToTarget]).Build());

    }

    // Each goal has a desired effect which is actually a belief that needs to be true to achieve the goal
    // The higher the priority the more likely is a goal to get executed
    private void SetupGoals()
    {
        goals = new HashSet<AgentGoal>();
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.Idle).WithPriority(1).WithDesiredEffect(beliefs[AgentBelief.BeliefType.Nothing]).Build());
       
        // collecting resources
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.CollectWood).WithPriority(10).WithDesiredEffect(beliefs[AgentBelief.BeliefType.workersAssignedToWood]).Build());
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.CollectStone).WithPriority(6).WithDesiredEffect(beliefs[AgentBelief.BeliefType.workersAssignedToStone]).Build());
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.CollectIron).WithPriority(6).WithDesiredEffect(beliefs[AgentBelief.BeliefType.workersAssignedToIron]).Build());

        // construct buildings
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.ConstructWoodWorkplace).WithPriority(9).WithDesiredEffect(beliefs[AgentBelief.BeliefType.HasWoodWorkplace]).Build());
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.ConstructStoneWorkplace).WithPriority(9).WithDesiredEffect(beliefs[AgentBelief.BeliefType.HasStoneWorkplace]).Build());
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.ConstructIronWorkplace).WithPriority(9).WithDesiredEffect(beliefs[AgentBelief.BeliefType.HasIronWorkplace]).Build());
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.ConstructBlacksmith).WithPriority(9).WithDesiredEffect(beliefs[AgentBelief.BeliefType.HasBlacksmith]).Build());
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.ConstructBarracks).WithPriority(9).WithDesiredEffect(beliefs[AgentBelief.BeliefType.HasBarracks]).Build());

        // recruiting

        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.RecruitTroops).WithPriority(8).WithDesiredEffect(beliefs[AgentBelief.BeliefType.HasEnoughTroops]).WithDesiredEffect(beliefs[AgentBelief.BeliefType.HasAvailableTroops]).Build());

        // battle
        goals.Add(new AgentGoal.Builder(AgentGoal.GoalType.AttackOpponent).WithPriority(15).WithDesiredEffect(beliefs[AgentBelief.BeliefType.troopsAssignedToTarget]).Build());
    }

    public void Update()
    {
        // Update the plan and current action if there is one
        if(currentAction == null)
        {
            Debug.Log("calculating any potential new plan");
            CalculatePlan();

            if (actionPlan != null && actionPlan.actions.Count > 0)
            {
                

                currentGoal = actionPlan.agentGoal;
                Debug.Log($"Goal: {currentGoal.goalType.ToString()} with {actionPlan.actions.Count} actions in plan" );
                currentAction = actionPlan.actions.Pop();
                Debug.Log($"Popped action: {currentAction.actionType.ToString()}");
                // Verirfy all precondition effects are true
                if(currentAction.preconditions.All(belief => belief.Evaluate()))
                {
                    currentAction.Start();

                }else
                {
                    Debug.Log("preconditions not met, clearing current action and goal");
                    currentAction = null;
                    currentGoal = null;
                }
            }
        }

        // If we have a current action, execute it
        if (actionPlan != null && currentAction != null)
        {
            currentAction.Update(Time.deltaTime);

            if (currentAction.complete)
            {
                Debug.Log($"{currentAction.actionType.ToString()} complete");
                currentAction.Stop();
                currentAction = null;

                if (actionPlan.actions.Count == 0)
                {
                    Debug.Log("Plan complete");
                    lastGoal = currentGoal;
                    currentGoal = null;
                    goapInteractor.DeselectWorkers();
                    goapInteractor.DeseletTroops();
                }
            }
        }
    }

    private void CalculatePlan()
    {
        var priorityLevel = currentGoal?.priority ?? 0;

        HashSet<AgentGoal> goalsToCheck = goals;

        // If we have a current goal, we only want to check goals with higher priority
        if(currentGoal != null)
        {
            Debug.Log("current goal exists, checking goals with higher priority");
            goalsToCheck = new HashSet<AgentGoal>(goals.Where(goal => goal.priority > priorityLevel));
        }

        var potentialPlan = goapPlanner.CreatePlan(this, goalsToCheck, lastGoal);
        if (potentialPlan != null)
        {
            actionPlan = potentialPlan;
        }
    }
}
