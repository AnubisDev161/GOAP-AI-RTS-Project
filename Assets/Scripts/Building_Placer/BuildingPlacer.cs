using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] States current_state;

    [SerializeField] BuildingPlacerUI building_placer_UI;
    Mesh buildingPreviewMesh;
    [SerializeField] Mesh buildingPreviewMeshBarracks;
    [SerializeField] Mesh buildingPreviewMeshWorkplace;
    [SerializeField] Mesh buildingPreviewMeshBlacksmith;

    [SerializeField] Material buildingPreviewMaterial;
    [SerializeField] Material cantBuildPreviewMaterial;
  
    [SerializeField] LayerMask clickLayer;

    [SerializeField] ResourcePack woodWorkplaceCost;
    [SerializeField] ResourcePack stoneWorkplaceCost;
    [SerializeField] ResourcePack ironWorkplaceCost;
    [SerializeField] ResourcePack blacksmithCost;
    [SerializeField] ResourcePack barracksCost;

    [SerializeField] Workplace woodWorkplacePrefab;
    [SerializeField] Workplace stoneWorkplacePrefab;
    [SerializeField] Workplace ironWorkplacePrefab;
    [SerializeField] Blacksmith blacksmithPrefab;
    [SerializeField] Barracks barracksPrefab;
    Building BuildingPrefab;
    
    [SerializeField] Faction faction;
    [SerializeField] Target.factionTypes factionType;
    public enum States
    {
        Folded,
        ShowPlacingUI,
        Placing

    }

    void Start()
    {
        current_state = States.Folded;
        building_placer_UI.ShowBuildingButtons(false);
        building_placer_UI.ShowOpenBuildingPlacerButton(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (current_state == States.Placing)
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickLayer))
            {
               
                Graphics.DrawMesh(buildingPreviewMesh, hit.point, Quaternion.identity, buildingPreviewMaterial, 0);
                
            }

            if (Input.GetMouseButtonDown(0))
            {
                TryConstructBuilding(faction, factionType, hit.point);

            } else if(Input.GetMouseButtonDown(1)) 
            {
                changeState(States.ShowPlacingUI);
            }

        }
     
    }

    public void TryConstructBuilding(Faction faction, Target.factionTypes factionType,  Vector3 position)
    {
        if (TryBuyBuilding(faction, BuildingPrefab.GetBuildingType()))
        {
            Building newBuilding = Instantiate(BuildingPrefab, position, BuildingPrefab.transform.rotation);
            newBuilding.Initialize(faction, factionType);
            faction.AddBuilding(newBuilding);
            changeState(States.ShowPlacingUI);
        }
    }
    public bool TryConstructBuilding(Faction faction, Target.factionTypes factionType, Building.BuildingTypes buildingType, Vector3 position)
    {
        SetBuildingPrefabByType(buildingType);
        if (TryBuyBuilding(faction, BuildingPrefab.GetBuildingType()))
        {
            Building newBuilding = Instantiate(BuildingPrefab, position, Quaternion.identity);
            newBuilding.Initialize(faction, factionType);
            faction.AddBuilding(newBuilding);

            return true;
        }
        return false;
    }

    public ResourcePack GetBuildingCost(Faction faction, Building.BuildingTypes buildingType)
    {
        ResourcePack buildingCost = GetWorkplaceCost(buildingType);
       return buildingCost;
    }

   public void OnWoodWorkplaceButtonClicked()
   {
        BuildingPrefab = woodWorkplacePrefab.GetComponent<Building>();
        buildingPreviewMesh = buildingPreviewMeshWorkplace;
        changeState(States.Placing);
   }

    public void OnStoneWorkplaceButtonClicked()
    {
        BuildingPrefab = stoneWorkplacePrefab.GetComponent<Building>();
        buildingPreviewMesh = buildingPreviewMeshWorkplace;
        changeState(States.Placing);
    }
    public void OnIronWorkplaceButtonClicked()
    {
        BuildingPrefab = ironWorkplacePrefab.GetComponent<Building>();
        buildingPreviewMesh = buildingPreviewMeshWorkplace;
        changeState(States.Placing);
    }
    public void OnBlacksmithButtonClicked()
    {
        BuildingPrefab = blacksmithPrefab.GetComponent<Building>();
        buildingPreviewMesh = buildingPreviewMeshBlacksmith;
        changeState(States.Placing);
    }
    public void OnBarracksButtonClicked()
    {
        BuildingPrefab = barracksPrefab.GetComponent<Building>();
        buildingPreviewMesh = buildingPreviewMeshBarracks;
        changeState(States.Placing);
    }

    private bool TryBuyBuilding(Faction faction, Building.BuildingTypes buildingType)
    {
       ResourcePack workplaceCost = GetWorkplaceCost(buildingType);
       if(faction.resourcePack >= workplaceCost)
       {
            faction.UpdateResourceAmount(-workplaceCost);
            return true;
       }
       return false;
    }

    private ResourcePack GetWorkplaceCost(Building.BuildingTypes buildingType)
    {
        ResourcePack resourcePack;
        switch (buildingType)
        { 
            case Building.BuildingTypes.WoodWorkplace:
                resourcePack = woodWorkplaceCost;
                break;

            case Building.BuildingTypes.StoneWorkplace:
                resourcePack = stoneWorkplaceCost;
                break;

            case Building.BuildingTypes.IronWorkplace:
                resourcePack = ironWorkplaceCost;
                break;

            case Building.BuildingTypes.Blacksmith:
                resourcePack = blacksmithCost;
                break;

            case Building.BuildingTypes.Barracks:
                resourcePack = blacksmithCost;
                break;
            default:
                resourcePack = new ResourcePack();
                break;
        }

        return resourcePack;

    }

    private void SetBuildingPrefabByType(Building.BuildingTypes buildingType)
    {
        switch (buildingType)
        {
            case Building.BuildingTypes.WoodWorkplace:
                BuildingPrefab = woodWorkplacePrefab;
                break;

            case Building.BuildingTypes.StoneWorkplace:
                BuildingPrefab = stoneWorkplacePrefab;
                break;

            case Building.BuildingTypes.IronWorkplace:
                BuildingPrefab = ironWorkplacePrefab;
                break;

            case Building.BuildingTypes.Blacksmith:
                BuildingPrefab = blacksmithPrefab;
                break;

            case Building.BuildingTypes.Barracks:
                BuildingPrefab = barracksPrefab;
                break;
            default:
                BuildingPrefab = null;
                break;
        }
    }

    public void OnOpenBuildingPlacerButtonClicked()
    { 
        changeState(States.ShowPlacingUI);
        building_placer_UI.ShowBuildingButtons(true);
        building_placer_UI.ShowOpenBuildingPlacerButton(false);
    }

    public void OnCloseBuildingPlacerButtonClicked()
    {
        changeState(States.Folded);
        building_placer_UI.ShowBuildingButtons(false);
        building_placer_UI.ShowOpenBuildingPlacerButton(true);
    }

    public void changeState(States state)
    {
        current_state = state;
    }
}
