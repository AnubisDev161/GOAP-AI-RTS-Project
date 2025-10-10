using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AssignWorkersToResource : IActionStrategy
{
    readonly IGoapInteractor goapInteractor;
    Resource.ResourceTypes desiredResource;
    public bool canPerform => !complete;
    public bool complete => goapInteractor.HasSelectedWorkers() == true;

    public AssignWorkersToResource(IGoapInteractor goapInteractor, Resource.ResourceTypes desiredResource)
    {
        this.goapInteractor = goapInteractor;
    }

    public void Start()
    {
        goapInteractor.AsssignWorkersToResources(desiredResource);
    }
}
