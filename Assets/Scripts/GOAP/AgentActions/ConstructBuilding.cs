using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ConstructBuilding : IActionStrategy
{
    readonly IGoapInteractor goapInteractor;
    readonly Building.BuildingTypes buildingTypeToConstruct;
    int oldBuildingAmount = 0;
    public bool canPerform => !complete;
    public bool complete => goapInteractor.HasBuildings(buildingTypeToConstruct) > oldBuildingAmount;

    public ConstructBuilding(IGoapInteractor goapInteractor, Building.BuildingTypes buildingType)
    {
        this.goapInteractor = goapInteractor;
        buildingTypeToConstruct = buildingType;
    }

    public void Start()
    {
        oldBuildingAmount = goapInteractor.HasBuildings(buildingTypeToConstruct);
        goapInteractor.TryConstructBuilding(buildingTypeToConstruct);
    }
}
