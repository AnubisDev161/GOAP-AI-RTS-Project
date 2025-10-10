using System;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public struct ResourcePack
{
    [field: SerializeField]
    public int wood { get; private set; }
    [field: SerializeField]
    public int stone { get; private set; }
    [field: SerializeField]
    public int iron { get; private set; }
    [field: SerializeField]
    public int ironPlates { get; private set; }

    public ResourcePack(int wood, int stone, int iron, int ironPlates)
    {
        
        this.wood = wood;
        this.stone = stone;
        this.iron = iron;
        this.ironPlates = ironPlates;
    }
    public ResourcePack(int wood, int stone, int iron)
    {
        this.wood = wood;
        this.stone = stone;
        this.iron = iron;
        this.ironPlates = 0;
    }

    // Add a <= operator
    public static ResourcePack operator +(ResourcePack resourcePack_a, ResourcePack resourcePack_b)
    {
        return new ResourcePack(resourcePack_a.wood + resourcePack_b.wood, resourcePack_a.stone + resourcePack_b.stone, resourcePack_a.iron + resourcePack_b.iron, resourcePack_a.ironPlates + resourcePack_b.ironPlates);
    }
    public static ResourcePack operator -(ResourcePack resourcePack_a, ResourcePack resourcePack_b)
    {
        return new ResourcePack(resourcePack_a.wood - resourcePack_b.wood, resourcePack_a.stone - resourcePack_b.stone, resourcePack_a.iron - resourcePack_b.iron, resourcePack_a.ironPlates - resourcePack_b.ironPlates);
    }
    public static ResourcePack operator -(ResourcePack resourcePack_a )
    {
        return new ResourcePack(-resourcePack_a.wood, -resourcePack_a.stone, -resourcePack_a.iron, -resourcePack_a.ironPlates);
    }

    public static bool operator <(ResourcePack resourcePack_a, ResourcePack resourcePack_b)
    {
        if (resourcePack_a.wood < resourcePack_b.wood && resourcePack_a.stone < resourcePack_b.stone && resourcePack_a.iron < resourcePack_b.iron && resourcePack_a.ironPlates < resourcePack_b.ironPlates)
        {  return true; 
        }
        else
        {
            return false;
        }
           
    }

    public static bool operator > (ResourcePack resourcePack_a, ResourcePack resourcePack_b)
    {
        if (resourcePack_a.wood > resourcePack_b.wood && resourcePack_a.stone > resourcePack_b.stone && resourcePack_a.iron > resourcePack_b.iron && resourcePack_a.ironPlates > resourcePack_b.ironPlates)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator >=(ResourcePack resourcePack_a, ResourcePack resourcePack_b)
    {
        if (resourcePack_a.wood >= resourcePack_b.wood && resourcePack_a.stone >= resourcePack_b.stone && resourcePack_a.iron >= resourcePack_b.iron && resourcePack_a.ironPlates >= resourcePack_b.ironPlates)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator <=(ResourcePack resourcePack_a, ResourcePack resourcePack_b)
    {
        if (resourcePack_a.wood <= resourcePack_b.wood && resourcePack_a.stone <= resourcePack_b.stone && resourcePack_a.iron <= resourcePack_b.iron && resourcePack_a.ironPlates <= resourcePack_b.ironPlates)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public struct EnemyInfromation
{
    [field: SerializeField]
    public int troopCount { get; private set; }

    public EnemyInfromation(int troopCount)
    {
        this.troopCount = troopCount;

    }
}

