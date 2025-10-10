using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// all actions have the same structure : 
// canPerform checks whehter the action can be executed
// complete checks whether the action has been completed, this always depends on the action's purpose 
public class SelectWorkers : IActionStrategy
{
    readonly IGoapInteractor goapInteractor;

    public bool canPerform => !complete;
    public bool complete => goapInteractor.HasSelectedWorkers() == true;

    public SelectWorkers(IGoapInteractor goapInteractor)
    {
           this.goapInteractor = goapInteractor;
    }

    public void Start()
    {
        goapInteractor.SelectWorkers();
    }
}
