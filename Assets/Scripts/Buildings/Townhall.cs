using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Townhall : Building, IClickable
{
    List<Resource> woodResourcesInRange = new List<Resource>();
    List<Resource> stoneResourcesInRange = new List<Resource>();
    List<Resource> ironResourcesInRange = new List<Resource>();


    [SerializeField] ResourcePack workerCost;
    [SerializeField] Worker workerPrefab;
    [SerializeField] Transform recruitSpawnPoint;
    [SerializeField] RecruitingBuildingUI ui;
    public static Action<factionTypes> FactionDestroyed;

    private void OnEnable()
    {
        Resource.removeResourceFromList += OnRemoveResourceFromList;
    }

    private void OnDisable()
    {
        Resource.removeResourceFromList -= OnRemoveResourceFromList;
    }


    public void OnRemoveResourceFromList(Resource resource)
    {
        if(woodResourcesInRange.Contains(resource))
        {
            woodResourcesInRange.Remove(resource);
        }
        else if (stoneResourcesInRange.Contains(resource))
        {
            stoneResourcesInRange.Remove(resource);
        }
        if (ironResourcesInRange.Contains(resource))
        {
            ironResourcesInRange.Remove(resource);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Resource>(out Resource resource))
        {
            switch (resource.GetResourceType())
            {
                case Resource.ResourceTypes.Wood:
                    woodResourcesInRange.Add(resource);
                    break;
                case Resource.ResourceTypes.Stone:
                    stoneResourcesInRange.Add(resource);
                    break;
                case Resource.ResourceTypes.Iron:
                    ironResourcesInRange.Add(resource);
                    break;
                default:
                    break;
            }
        }
    }


    public GameObject find_closest_Resource_in_range(Resource.ResourceTypes requestedResource)
    {
        // TODO Expand the view range of the townhall to find new resources when the current range is cleaned
        var resource = FindResource(GetRequestedResourceList(requestedResource));

        if(resource == null)
        {
            faction.AllResourcesOfSpecificTypeCollected(requestedResource);
        }
        return resource;
    }

    public GameObject find_closest_target_in_range()
    {
        // TODO Expand the view range of the townhall to find new resources when the current range is cleaned
        var target = FindTarget(GetTargetList());

        if (target == null)
        {
            faction.NoTargetsAvailable();
        }
        return target;
    }

    private List<Resource> GetRequestedResourceList(Resource.ResourceTypes requestedResource)
    {
        switch (requestedResource)
        {
            case(Resource.ResourceTypes.Wood):
                return woodResourcesInRange;
                break;
            case(Resource.ResourceTypes.Stone):
                return stoneResourcesInRange;
                break;
            case(Resource.ResourceTypes.Iron):
                return ironResourcesInRange;
                break;
            default :
            return null;
                break;
        }
    }

    private List<GameObject> GetTargetList()
    {
        return faction.GetAllAvailableTargets();
    }

    private GameObject FindResource(List<Resource> resourcesInRange)
    {
        if (resourcesInRange == null || resourcesInRange.Count == 0) return null;
        var shortest_distance_to_resource = math.INFINITY;
        var distance_to_resource = 0f;
        Resource closest_resource = null;

        foreach (var resource in resourcesInRange)
        {
            if(resource == null) continue;
            distance_to_resource = Mathf.Abs((resource.transform.position - transform.position).magnitude);
            if(distance_to_resource < shortest_distance_to_resource)
            {
                shortest_distance_to_resource = distance_to_resource;
                closest_resource = resource;
            }
        }

        if (closest_resource != null)
        {
            return closest_resource.gameObject;
        }
        else
        {
            return null;
        }
    }

    private GameObject FindTarget(List<GameObject> targetsInRange)
    {
        if (targetsInRange == null || targetsInRange.Count == 0) return null;
        var shortest_distance_to_target = math.INFINITY;
        var distance_to_target = 0f;
        GameObject closest_target = null;

        foreach (var target in targetsInRange)
        {
            if (target == null) continue;
            distance_to_target = Mathf.Abs((target.transform.position - transform.position).magnitude);
            if (distance_to_target < shortest_distance_to_target)
            {
                shortest_distance_to_target = distance_to_target;
                closest_target = target;
            }
        }

        if (closest_target != null)
        {
            return closest_target.gameObject;
        }
        else
        {
            return null;
        }
    }


    public void OnWorkerButtonClicked()
    {
        TryRecruitWorker(Target.factionTypes.player);
    }

    public void ShowUI(bool show)
    {
        ui.ShowButtons(show);
    }

    public bool TryRecruitWorker(Target.factionTypes factionType)
    {
        if (faction.HasFactionEnoughResources(workerCost))
        {
            faction.UpdateResourceAmount(-workerCost);
            Worker newUnit = Instantiate(workerPrefab, recruitSpawnPoint.position, Quaternion.identity);
            newUnit.factionType = factionType;
            newUnit.faction = faction;
            faction.AddWorker(newUnit);
            return true;
        }
        return false;
    }

    public bool CanFactionRecruitWoker(Target.factionTypes factionType)
    {
        if (faction.HasFactionEnoughResources(workerCost))
        { 
            return true;
        }
        return false;
    }
    public void AddResourcesInRange(Resource resource)
    {
        switch (resource.GetResourceType())
        {
            case Resource.ResourceTypes.Wood:
                woodResourcesInRange.Add(resource);
                break;
            case Resource.ResourceTypes.Stone:
                stoneResourcesInRange.Add(resource);
                break;
            case Resource.ResourceTypes.Iron:
                ironResourcesInRange.Add(resource);
                break;
            default:
                break;
        }
    }

    public override void die()
    {
        FactionDestroyed?.Invoke(factionType);
        base.die();
    }
}


