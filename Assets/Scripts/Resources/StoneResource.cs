using UnityEngine;

public class StoneResource : Resource
{
    [SerializeField] int stone_amount = 100;

    private void Awake()
    {
        resourcePack = new ResourcePack(0, stone_amount, 0, 0);
    }
}


