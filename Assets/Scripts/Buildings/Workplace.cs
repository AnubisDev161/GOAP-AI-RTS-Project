using System;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : Building, IClickable
{
    [SerializeField]
    Resource.ResourceTypes workplaceType;
    public bool IsWorkplaceAppropriateForResource(Resource resource)
    {
        if (workplaceType == resource.GetResourceType())
        {
            return true;
        }
        return false; 
    }

    public Resource.ResourceTypes GetResourceType()
    {
        return workplaceType;
    }

    public GameObject RequestNewTargetResource()
    {
        return faction.RequestNewTargetResource(workplaceType);
    }
}
