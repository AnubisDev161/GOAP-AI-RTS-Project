using System.Collections.Generic;
using UnityEngine;

public class FactionDetectionArea : MonoBehaviour
{
    [SerializeField] Townhall townhall;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Resource>(out Resource resource))
        {
            townhall.AddResourcesInRange(resource);

        }
       
    }

}
