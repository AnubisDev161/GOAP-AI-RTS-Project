using System;
using System.Collections.Generic;
using UnityEngine;
public class BuildingPositionContainer : MonoBehaviour
{
    [SerializeField] List<GameObject> buildingPostitions;

    public Vector3 GetBuildingPositionByType(Building.BuildingTypes buildingType)
    {
        Vector3 buildingPosition = Vector3.zero;
        switch (buildingType)
        {
            case Building.BuildingTypes.WoodWorkplace:
                buildingPosition = buildingPostitions[0].transform.position;
                break;

            case Building.BuildingTypes.StoneWorkplace:
                buildingPosition = buildingPostitions[1].transform.position;
                break;

            case Building.BuildingTypes.IronWorkplace:
                buildingPosition = buildingPostitions[2].transform.position;
                break;

            case Building.BuildingTypes.Blacksmith:
                buildingPosition = buildingPostitions[3].transform.position;
                break;

            case Building.BuildingTypes.Barracks:
                buildingPosition = buildingPostitions[4].transform.position;
                break;
            default:
                buildingPosition = Vector3.zero;
                break;
     
        }
        return buildingPosition;    
    }
}

