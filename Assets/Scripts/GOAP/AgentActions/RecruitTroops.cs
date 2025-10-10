using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class RecruitTroops : IActionStrategy
{
    readonly IGoapInteractor goapInteractor;
    readonly Building.BuildingTypes buildingTypeToConstruct;
    int desiredTroopsAmount;
    public bool canPerform => !complete;
    public bool complete => goapInteractor.HasAvailableTroops() >= desiredTroopsAmount || !goapInteractor.CanFactionAffordTroops(desiredTroopsAmount);

    public RecruitTroops(IGoapInteractor goapInteractor, int desiredTroopsAmount)
    {
        this.goapInteractor = goapInteractor;
        this.desiredTroopsAmount = desiredTroopsAmount;
    }

    public void Start()
    {
        goapInteractor.TryRecruitTroops(desiredTroopsAmount, Target.factionTypes.CPU);
    }
}
