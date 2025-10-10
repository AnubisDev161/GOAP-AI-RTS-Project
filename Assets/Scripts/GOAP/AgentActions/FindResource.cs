using UnityEngine;
public class FindResource : IActionStrategy
{
    readonly IGoapInteractor goapInteractor;
    public bool canPerform => !complete;
    public bool complete => wantedResource != null || noResourcesFound;

    Resource.ResourceTypes wantedResourceType;
    GameObject wantedResource = null;
    bool noResourcesFound = false;

    public FindResource(IGoapInteractor goapInteractor, Resource.ResourceTypes wantedResourceType)
    {
        this.goapInteractor = goapInteractor;
        this.wantedResourceType = wantedResourceType;
    }

    public void Start()
    {
        wantedResource = (goapInteractor.FindResource(wantedResourceType));
        if (wantedResource == null)
        {
            noResourcesFound = true;
        }
    }
}

