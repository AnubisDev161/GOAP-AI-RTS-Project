using System.Resources;
using UnityEngine;

public class TreeResource : Resource
{
    [SerializeField] int wood_amount = 100;

    private void Awake()
    {
        resourcePack = new ResourcePack(wood_amount, 0, 0, 0);
    }
   

}


   