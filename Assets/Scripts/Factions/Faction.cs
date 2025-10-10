using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
public class Faction : MonoBehaviour, IGoapInteractor
{
    [SerializeField] Target.factionTypes factionType;
    [SerializeField] public ResourcePack resourcePack = new ResourcePack();
    [SerializeField] List<Workplace> workplaces;
    [SerializeField] List<Blacksmith> blacksmiths;
    [SerializeField] List<Barracks> barracks;
    [SerializeField] List<Townhall> townhalls;
    [SerializeField] LayerMask clickLayer;
    Building selectedBuilding;
    [SerializeField] BuildingPlacer buildingPlacer;
    [SerializeField] BuildingPositionContainer buildingPositions;

    ResourceManager resourceManager = new ResourceManager();
    [SerializeField] List<Worker> selectedWorkers;
    [SerializeField] List<Worker> busyWorkers;
    [SerializeField] List<Worker> waitingWorkers;


    [SerializeField] List<Unit> selectedTroops;
    [SerializeField] List<Unit> busyTroops;
    [SerializeField] List<Unit> waitingTroops;

    [SerializeField] List<GameObject> availableTargets;

    // the GOAP interactor interface implements all methods needed to give the GOAP agent access to world data and to upate its beliefs

    private void OnEnable()
    {
        Target.newTargetAvailable += OnNewTargetAvailable;
        Target.removeUnitFromList += OnRemoveActorFromList;
    }

    private void OnRemoveActorFromList(GameObject actor)
    {
        if(actor.TryGetComponent<Unit>(out Unit unit))
        {
            if (busyTroops.Contains(unit))
            {
                busyTroops.Remove(unit);
            }
            else if (waitingTroops.Contains(unit))
            {
                waitingTroops.Remove(unit);
            }
            else if (selectedTroops.Contains(unit))
            {
                selectedTroops.Remove(unit);
            }
        }
       else if(actor.TryGetComponent<Worker>(out Worker worker))
       {
            if (busyWorkers.Contains(worker))
            {
                busyWorkers.Remove(worker);
            }
            else if (waitingWorkers.Contains(worker))
            {
                waitingWorkers.Remove(worker);
            }
            else if (selectedWorkers.Contains(worker))
            {
                selectedWorkers.Remove(worker);
            }
       }

        if (availableTargets.Contains(actor))
        {
            availableTargets.Remove(actor);
        }
    }

    private void OnDisable()
    {
        Target.newTargetAvailable -= OnNewTargetAvailable;
        Target.removeUnitFromList -= OnRemoveActorFromList;
    }

    public void OnRemoveAvaiableTarget(GameObject target, Faction faction)
    {
        if (faction == this) return;
        if (availableTargets.Contains(target))
        {
            availableTargets.Remove(target);
        }
    }
    public void OnNewTargetAvailable(GameObject potential_target, Faction faction)
    {
        if (faction == this) return;
        if ((potential_target.TryGetComponent<Target>(out Target target)))
        {
            availableTargets.Add(potential_target);
        }
    }

    public enum factionTypes
    {
        None,
        Player,
        CPU
    }

    public void UpdateResourceAmount(ResourcePack resourcePack)
    {
        this.resourcePack += resourcePack;
    }

    public List<Workplace> GetWorkplaces()
    {
        return workplaces;
    }

    public void RemoveWorkplaces(Workplace workplaceToRemove)
    {
        if (workplaces.Contains(workplaceToRemove))
        {
            workplaces.Remove(workplaceToRemove);
        }

    }

    public GameObject RequestNewTargetResource(Resource.ResourceTypes resourceType)
    {
        foreach (var townhall in townhalls)
        {
            return townhall.find_closest_Resource_in_range(resourceType);
        }
        return null;
    }

    public void AllResourcesOfSpecificTypeCollected(Resource.ResourceTypes resourceType)
    {
        Debug.Log("All resources in range of Type : " + ($"{resourceType.ToString()} collected"));
    }

    public void NoTargetsAvailable()
    {
        Debug.Log("No more targets in range");
    }

    public void AddBuilding(Building newBuilding)
    {
        if (newBuilding.TryGetComponent<Workplace>(out Workplace workplace))
        {
            workplaces.Add(workplace);
        }
        else if (newBuilding.TryGetComponent<Blacksmith>(out Blacksmith blacksmith))
        {
            blacksmiths.Add(blacksmith);
        }
        else if (newBuilding.TryGetComponent<Barracks>(out Barracks barracks))
        {
            this.barracks.Add(barracks);
        }
    }

    public void AddWorker(Worker workerToAdd)
    {
        waitingWorkers.Add(workerToAdd);
    }

    public void AddTroop(Unit unitToAdd)
    {
        waitingTroops.Add(unitToAdd);
    }

    public void RemoveWorkerFromBusyList(Worker workerOutOfTasks)
    {
        if(busyWorkers.Contains(workerOutOfTasks) && !busyWorkers.Contains(workerOutOfTasks))
        {
            busyWorkers.Remove(workerOutOfTasks);
            waitingWorkers.Add(workerOutOfTasks);
        }
    }

    public bool HasFactionEnoughResources(ResourcePack resourcePack)
    {
        return this.resourcePack >= resourcePack;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickLayer))
        {

            if (Input.GetMouseButtonDown(0))
            {
                CheckMouseHitClickableObject(hit);
            }
        }
    }

    public void CheckMouseHitClickableObject(RaycastHit hit)
    {
        if (hit.collider.TryGetComponent<IClickable>(out IClickable clickableComponent))
        {
            clickableComponent.OnClick();
            if (clickableComponent.get_parent().TryGetComponent<Building>(out Building building))
            {
                selectedBuilding = building;
            }
        }
        else
        {
            if (selectedBuilding == null) return;

            selectedBuilding.OnDeselect();
            selectedBuilding = null;
        }
    }

    public int HasAvailableWorkers()
    {
        return waitingWorkers.Count;
    }

    public List<Worker> SelectWorkers()
    {
        foreach (Worker worker in waitingWorkers)
        {
            if (!selectedWorkers.Contains(worker))
            {
                selectedWorkers.Add(worker);
            }
        }
        return selectedWorkers;
    }

    public bool HasSelectedWorkers()
    {
        if (selectedWorkers.Count == 0) return false;

        return true;
    }

    public GameObject FindResource(Resource.ResourceTypes resourceType)
    {
        SetSearchedResource(RequestNewTargetResource(resourceType));
        if(VerifyTargetResource())
        {
            return resourceManager.searchedResource;
        }
    
        return null;
    }

    private bool VerifyTargetResource()
    {
        if (resourceManager.searchedResource == null) return false;
        foreach (var worker in busyWorkers)
        {
            if (resourceManager.searchedResource == worker.target) return false;
        }
        return true;    
    }

    private bool VerifyTarget(Target target)
    {
        foreach (var troop in busyTroops)
        {
            if (target == troop.target) return false;
        }
        return true;
    }

    private GameObject SetSearchedResource(GameObject searchedResource)
    {
        if (searchedResource == null) return null;

        resourceManager.searchedResource = searchedResource;
        resourceManager.searchedResourceType = searchedResource.GetComponent<Resource>().GetResourceType();
        return searchedResource;
    }

    public Resource GetSearchedResource(Resource.ResourceTypes resourceType)
    {
        if (resourceManager.searchedResource == null) return null;

        if (resourceManager.searchedResource.TryGetComponent<Resource>(out Resource resource))
        {
            if (resource.GetResourceType() == resourceType)
            {
                return resource;
            }
        }
        return null;
    }

    // Methods like these are called by the various actions the GOAP agent can perform
    public void AsssignWorkersToResources(Resource.ResourceTypes targetResource)
    {
        foreach (var worker in waitingWorkers)
        {
            worker.set_nav_agent_destination_and_target(resourceManager.searchedResource);
            worker.order_placed = true;
            busyWorkers.Add(worker);
        }

        foreach (var worker in busyWorkers)
        {
            waitingWorkers.Remove(worker);
        }
    }

    public void AsssignWorkersToResources(Resource.ResourceTypes targetResource, int workerToAssignCount)
    {
        if(workerToAssignCount > waitingWorkers.Count)
        {
            workerToAssignCount = waitingWorkers.Count;
        }
        
        for (int i = 0; i < workerToAssignCount; i++)
        {
            waitingWorkers[i].set_nav_agent_destination_and_target(resourceManager.searchedResource);
            waitingWorkers[i].order_placed = true;
            busyWorkers.Add(waitingWorkers[i]);
        }
    }
    // a classic method used to update the beliefs of the agent
    // in this example the agent can find out how many workers are assigned to a specific resource via this method
    public int GetWorkersAssignedToResourceCount(Resource.ResourceTypes targetResource)
    {
        int workersAssignedTargetResource = 0;
        foreach (var worker in busyWorkers)
        {
            if (worker.target != null && worker.target.TryGetComponent<Resource>(out Resource resource))
            {
                if (resource.GetResourceType() == targetResource)
                {
                    workersAssignedTargetResource++;

                }
            }
        }
        return workersAssignedTargetResource;
    }

    public ResourcePack GetResourceAmount()
    {
        resourceManager.lastResourceAmount = resourcePack;
        return resourcePack;
    }

    public ResourcePack GetLastResourceAmount()
    {
        return resourceManager.lastResourceAmount;
    }

    public void DeselectWorkers()
    {
        foreach (Worker worker in busyWorkers)
        {
            if (selectedWorkers.Contains(worker))
            {
                selectedWorkers.Remove(worker);
            }
        }
    }

    public bool TryConstructBuilding(Building.BuildingTypes buildingType)
    {
        if (buildingPlacer.TryConstructBuilding(this, factionType, buildingType, buildingPositions.GetBuildingPositionByType(buildingType)))
        {
            return true;
        }

        return false;
    }

    public bool TryRecruitWorker(Target.factionTypes factionType)
    {
        var townhall = townhalls.First();
        bool canRecruitWorker = false;
        if (townhall.TryRecruitWorker(factionType))
        {
            canRecruitWorker = true;
        }

       return canRecruitWorker;
    }

    public ResourcePack GetBuildingCost(Building.BuildingTypes buildingType)
    {
        return buildingPlacer.GetBuildingCost(this, buildingType);
       
    }

    public bool CanFactionAffordWorker(Target.factionTypes factionType)
    {
        var townhall = townhalls.First();
        bool canRecruitWorker = false;
        if (townhall.CanFactionRecruitWoker(factionType))
        {
            canRecruitWorker = true;
        }

        return canRecruitWorker;
    }

    public int HasBuildings(Building.BuildingTypes buildingType)
    {
        var wantedType = 0;
        
        foreach (Building workplace in workplaces)
        { 
            if (workplace.GetBuildingType() == buildingType)
            {
                wantedType++;
            }
        }

        foreach (Building baracks in barracks)
        {
            if (baracks.GetBuildingType() == buildingType)
            {
                wantedType++;
            }
        }

        foreach (Building baracks in blacksmiths)
        {
            if (baracks.GetBuildingType() == buildingType)
            {
                wantedType++;
            }
        }

        return wantedType;
    }

    public int HasAvailableTroops()
    {
        return waitingTroops.Count;
    }

    public bool HasSelectedTroops()
    {
        if (selectedTroops.Count == 0) return false;

        return true;
    }

    public bool TryRecruitTroops(int desiredTroopssAmount, Target.factionTypes factionType)
    {
        var _barracks = barracks.First();
        bool canRecruitTroops = false;
        for (int i = 0; i < desiredTroopssAmount; i++)
        {
            if (_barracks.TryRecruitTroop(factionType))
            {
                canRecruitTroops = true;
            }
        }

        return canRecruitTroops;
    }

    public bool CanFactionAffordTroops(int desiredTroopsAmount)
    {
        if (barracks.Count == 0) return false;

        var _barracks = barracks.First();
        bool canRecruitTroops = false;

        for (int i = 0; i < desiredTroopsAmount; i++)
        {
            if (_barracks.CanFactionAffordTroops())
            {
                canRecruitTroops = true;
            }
        }

        return canRecruitTroops;
    }

    public List<Unit> SelectTroops()
    {
        foreach (Unit troop in waitingTroops)
        {
            if (!selectedTroops.Contains(troop))
            {
                selectedTroops.Add(troop);
            }
        }
        return waitingTroops;
    }
    

    public void DeseletTroops()
    {
        foreach (Unit troop in busyTroops)
        {
            if (selectedTroops.Contains(troop))
            {
                selectedTroops.Remove(troop);
            }

        }
    }

    public void AsssignTroopsToTargets(GameObject potentialTarget)
    {
        if(potentialTarget == null) return;

        if (potentialTarget.TryGetComponent<Target>(out Target target))
        {
            foreach (var troop in waitingTroops)
            {
                troop.set_nav_agent_destination_and_target(potentialTarget.transform.position, target.gameObject);
                busyTroops.Add(troop);

            }

            foreach (var troop in busyTroops)
            {
                waitingTroops.Remove(troop);

            } 
        }
    }

    public int GetTroopsAssignedTargetsCount(factionTypes opponentFaction)
    {
        int troopsAssingedToTargets = 0;
        foreach (var troops in busyTroops)
        {
            if (troops.target != null && troops.target.TryGetComponent<Target>(out Target opponentTroop))
            {
                troopsAssingedToTargets++;
            }
        }
        return troopsAssingedToTargets;
    }

    public List<GameObject> GetAllAvailableTargets()
    {
        return availableTargets;
    }

    public GameObject FindTarget()
    {
        if(availableTargets.Count == 0) return null;

        var potentialTarget = RequestTarget();
        if (potentialTarget != null && potentialTarget.TryGetComponent<Target>(out Target target) && VerifyTarget(target))
        {
            return potentialTarget;
        }

        return null;
    }

    public GameObject RequestTarget()
    {
        foreach (var townhall in townhalls)
        {
            return townhall.find_closest_target_in_range();
        }
        return null;
    }

    public int GetAllWorkersCount()
    {
        return busyWorkers.Count + waitingWorkers.Count;
    }
}

public class ResourceManager
{
    public GameObject searchedResource;
    public Resource.ResourceTypes searchedResourceType;
    public ResourcePack lastResourceAmount;
}
