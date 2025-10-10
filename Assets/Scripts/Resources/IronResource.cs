using UnityEngine;

public class IronResource : Resource
{
    [SerializeField] int iron_amount = 100;
    private void Awake()
    {
        resourcePack = new ResourcePack(0, 0, iron_amount, 0);
    }

}


