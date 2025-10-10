using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WanderStrategy : IActionStrategy
{
    readonly NavMeshAgent navMeshAgent;
    readonly float wanderRadius;

    public bool canPerform => !complete;

    public bool complete => navMeshAgent.remainingDistance <= 2f && !navMeshAgent.pathPending;

    public WanderStrategy(NavMeshAgent navMeshAgent, float wanderRadius)
    {
        this.navMeshAgent = navMeshAgent;
        this.wanderRadius = wanderRadius;
    }

    public void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomDirection = (UnityEngine.Random.insideUnitSphere * wanderRadius);
            NavMeshHit hit;

            if (NavMesh.SamplePosition(navMeshAgent.transform.position + randomDirection, out hit, wanderRadius, 1))
            {
                navMeshAgent.destination = hit.position;
                return;
            }
        }
    }
}
