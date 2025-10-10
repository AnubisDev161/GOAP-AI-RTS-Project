using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

// The initial idea was to create an interface for all classes that can interact with a GOAP agent
// in the end I'm not sure anymore whether this is the right approach
public interface IGoapInteractor
{
    // troops
    public bool CanFactionAffordTroops(int desiredTroopsAmount);
    public bool TryRecruitTroops(int desiredTroopssAmount, Target.factionTypes factionType);
    void AsssignTroopsToTargets(GameObject potentialTarget);
    public int GetTroopsAssignedTargetsCount(Faction.factionTypes opponentFaction);
    public int HasAvailableTroops();
    public GameObject FindTarget();
    public bool HasSelectedTroops();
    List<Unit> SelectTroops();
    void DeseletTroops();

    // battle

    public List<GameObject> GetAllAvailableTargets();

    // workers
    public bool CanFactionAffordWorker(Target.factionTypes factionType);
    public bool TryRecruitWorker(Target.factionTypes factionType);
    void AsssignWorkersToResources(Resource.ResourceTypes targetResource);
    public int GetWorkersAssignedToResourceCount(Resource.ResourceTypes targetResource);
    public int GetAllWorkersCount();
    public bool HasSelectedWorkers();
    public int HasAvailableWorkers();
    List<Worker> SelectWorkers();
    void DeselectWorkers();

    // resources
    ResourcePack GetResourceAmount();
    ResourcePack GetLastResourceAmount();
    public bool HasFactionEnoughResources(ResourcePack resourcePack);
    public Resource GetSearchedResource(Resource.ResourceTypes resourceType);
    public GameObject FindResource(Resource.ResourceTypes resourceType);

    // buildings
    bool TryConstructBuilding(Building.BuildingTypes buildingType);
    ResourcePack GetBuildingCost(Building.BuildingTypes buildingType);
    int HasBuildings(Building.BuildingTypes buildingType);
}

