using System;
using System.Collections;
using UnityEngine;

public class Resource : MonoBehaviour,  IClickable
{
    [SerializeField] ResourceTypes resourceType;
    [SerializeField] protected ResourcePack resourcePack;
    public static Action<Resource> removeResourceFromList;

    public enum ResourceTypes
    {
        None,
        Wood,
        Stone,
        Iron
    }

    
    public void OnClick()
    {
        
    }

    public virtual ResourcePack TryCollect()
    {
        return resourcePack;
    }

    public virtual ResourcePack Collect()
    {
        var resourchePack = resourcePack;
        removeResourceFromList?.Invoke(this);
        Destroy(gameObject);
        return resourchePack;
    }

    public GameObject get_parent()
    {
       return this.gameObject;
    }

    public ResourceTypes GetResourceType()
    {
        return resourceType;
    }

    public void OnDeselect()
    {
        
    }
}
