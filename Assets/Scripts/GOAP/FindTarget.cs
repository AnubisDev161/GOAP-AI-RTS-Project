using UnityEngine;

internal class FindTarget : IActionStrategy
{
    readonly IGoapInteractor goapInteractor;
    public bool canPerform => !complete;
    public bool complete => potentialTarget != null;

    GameObject potentialTarget = null;

    public FindTarget(IGoapInteractor goapInteractor)
    {
        this.goapInteractor = goapInteractor;
    }

    public void Start()
    {
        potentialTarget = (goapInteractor.FindTarget());
    }
}