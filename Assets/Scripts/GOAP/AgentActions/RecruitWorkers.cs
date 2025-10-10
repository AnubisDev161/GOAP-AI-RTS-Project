using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RecruitWorkers : IActionStrategy
{
    readonly IGoapInteractor goapInteractor;
    readonly Building.BuildingTypes buildingTypeToConstruct;
    int desiredWorkersAmount;
    public bool canPerform => !complete;
    public bool complete => recruitingWasSuccessful || recruitingFailed;

    bool recruitingWasSuccessful;
    bool recruitingFailed;
    public RecruitWorkers(IGoapInteractor goapInteractor, int desiredWorkersAmount)
    {
        this.goapInteractor = goapInteractor;
        this.desiredWorkersAmount = desiredWorkersAmount;
    }

    public void Start()
    {
       recruitingWasSuccessful = goapInteractor.TryRecruitWorker(Target.factionTypes.CPU);
        recruitingFailed = !recruitingWasSuccessful;
    }
}
