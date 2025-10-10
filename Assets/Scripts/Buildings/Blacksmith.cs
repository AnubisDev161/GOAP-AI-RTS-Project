using UnityEngine;

public class Blacksmith : Building
{
    [SerializeField] int productionRate;
    [SerializeField] float productionTime;
    float currentProductionTime = 0.0f;
    // Update is called once per frame
    void Update()
    {
        currentProductionTime += Time.deltaTime;
        if (currentProductionTime >= productionTime)
        {
            ProduceResource();
            currentProductionTime = 0.0f;
        }
    }

    void ProduceResource()
    {
        if (faction.HasFactionEnoughResources(new ResourcePack(0, 0, productionRate, 0)))
        {
            faction.UpdateResourceAmount(new ResourcePack(0, 0, -productionRate, productionRate));
        }   
    }
}
