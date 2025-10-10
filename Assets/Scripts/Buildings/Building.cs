using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Building : Target, IClickable, IDamageable
{
    [SerializeField]
    protected BuildingTypes buildingType;

    [Header("Health")]
    [field: SerializeField] public float max_health { get; set; } = 100;
    [field: SerializeField] public float current_health { get; set; } = 100;

    public enum BuildingTypes
    {
        WoodWorkplace,
        StoneWorkplace,
        IronWorkplace,
        Blacksmith,
        Barracks,
        Townhall
    }

    private void Start()
    {
        newTargetAvailable?.Invoke(gameObject, faction);
    }

    public BuildingTypes GetBuildingType()
    {
        return buildingType;
    }

    public virtual void Initialize(Faction faction, Target.factionTypes factionType)
    {
        this.faction = faction;
        this.factionType = factionType;
    }

    public GameObject get_parent()
    {
        return this.gameObject;
    }

    public void SetFaction(Faction faction)
    {
        this.faction = faction;
    }

    public virtual void OnClick()
    {
       
    }

    public virtual void OnDeselect()
    {
        
    }

    public void update_health(float increment, Unit attacker)
    {
        current_health += increment;
        if (current_health <= 0f)
        {
            die();
        }
    }

    public virtual void die()
    {
        dead = true;
        removeUnitFromList?.Invoke(gameObject);
        Destroy(gameObject);
    }
}
