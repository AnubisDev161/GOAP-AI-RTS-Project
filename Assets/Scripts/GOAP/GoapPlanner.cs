using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public interface IGoapPlanner
{
    ActionPlan CreatePlan(GoapAgent goapAgent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null);
}
public class GoapPlanner : IGoapPlanner
{
    public ActionPlan CreatePlan(GoapAgent goapAgent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null)
    {
        // Order goals by priority, descending
        List<AgentGoal> orderedGoals = goals
            .Where(goal => goal.desiredEffects.Any(belief => !belief.Evaluate())).
            OrderByDescending(goal => goal == mostRecentGoal ? goal.priority - 0.01 : goal.priority).ToList(); 

        // Try to solve each goal in order
        foreach (var goal in orderedGoals)
        {
            GoapNode goalNode = new GoapNode(null, null, goal.desiredEffects, 0);

            // If we can find a path to the goal, return the plan
            if(FindPath(goalNode, goapAgent.actions))
            {
                // If the goalNode has no leaves and no action to perform try a different goal
                if(goalNode.IsLeafDead) continue;

                Stack<AgentAction> actionStack = new Stack<AgentAction>();
                while (goalNode.leaves.Count > 0)
                {
                    var cheapestLeafNode = goalNode.leaves.OrderBy(leaf => leaf.cost).First();
                    goalNode = cheapestLeafNode;
                    actionStack.Push(cheapestLeafNode.action);
                }

                return new ActionPlan(goal, actionStack, goalNode.cost);
            }
        }

        Debug.LogWarning("No plan found");
        return null;
    }

    private bool FindPath(GoapNode parentNode, HashSet<AgentAction> actions)
    {
        foreach (var action in actions)
        {
            var requiredEffects = parentNode.requiredEffects;

            // Remove any effects that evaluate to true, because there is no action to take
            requiredEffects.RemoveWhere(belief => belief.Evaluate());

            // If there are no required effects to fulfill, we have a plan
            if (requiredEffects.Count() == 0)
            {
                return true;
            }

            if (action.effects.Any(requiredEffects.Contains))
            {
                var newRequiredEffects = new HashSet<AgentBelief>(requiredEffects);
                newRequiredEffects.ExceptWith(action.effects);
                newRequiredEffects.UnionWith(action.preconditions);

                // This part my cause problems in a more complex context. consider removing...
                var newAvailableActions = new HashSet<AgentAction>(actions);
                newAvailableActions.Remove(action);

                // section end


                var newNode = new GoapNode(parentNode, action, newRequiredEffects, parentNode.cost + action.cost);

                // Explore the new node recursively
                if(FindPath(newNode, newAvailableActions))
                {
                    parentNode.leaves.Add(newNode);
                    newRequiredEffects.ExceptWith(newNode.action.preconditions);

                }

                // if all effects at this depth have been satisfied, return true
                if(newRequiredEffects.Count() == 0)
                {
                    return true;
                }
            }
        }

        return false;
    }
}

public class GoapNode
{
    public GoapNode parent {  get; }
    public AgentAction action { get; }
    public HashSet<AgentBelief> requiredEffects { get; }
    public List<GoapNode> leaves { get; }
    public float cost {  get; }

    public bool IsLeafDead => leaves.Count == 0 && action == null;

    public GoapNode(GoapNode parent, AgentAction action, HashSet<AgentBelief> requiredEffects, float cost)
    {
        this.parent = parent;
        this.action = action;
        this.requiredEffects = new HashSet<AgentBelief>(requiredEffects);
        this.leaves = new List<GoapNode>();
        this.cost = cost;
    }
}

