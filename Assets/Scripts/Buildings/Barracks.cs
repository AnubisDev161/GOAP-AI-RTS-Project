using Unity.VisualScripting;
using UnityEngine;

public class Barracks : Building, IClickable
{
    [SerializeField] ResourcePack meleeUnitCost;
    [SerializeField] ResourcePack longRangeUnitCost;

    [SerializeField] Unit meleeUnitPrefab;
    [SerializeField] Long_Range_Unit longRangeUnitPrefab;
    Unit unitPrefab;

    [SerializeField] Barracks_UI ui;
    [SerializeField] Transform recruitSpawnPoint;
    [SerializeField]

    public void OnUnitButtonClicked()
    {
        unitPrefab = meleeUnitPrefab;
        TryRecruitTroop(Target.factionTypes.player);
    }
    public void OnLongRangeUnitButtonClicked()
    {
        unitPrefab = longRangeUnitPrefab;
        TryRecruitTroop(Target.factionTypes.player);
    }

    public void ShowUI(bool show)
    {
        ui.ShowButtons(show);
    }

    public bool TryRecruitTroop(Target.factionTypes factionType)
    {
        if (faction.HasFactionEnoughResources(GetRecruitingCost()))
        {
            faction.UpdateResourceAmount(-GetRecruitingCost());
            Unit newUnit = Instantiate(unitPrefab, recruitSpawnPoint.position, Quaternion.identity);
            newUnit.factionType = factionType;
            newUnit.faction = faction;
            faction.AddTroop(newUnit);
            return true;
        }
        return false;
    }

    public bool CanFactionAffordTroops()
    {
        unitPrefab = meleeUnitPrefab;

        if (faction.HasFactionEnoughResources(GetRecruitingCost()))
        { 
            return true;
        }
        return false;
    }

    ResourcePack GetRecruitingCost()
    {
        if (unitPrefab == meleeUnitPrefab)
        {
            return meleeUnitCost;
        }
        else
        {
            return longRangeUnitCost;
        }
    }
    public override void OnClick()
    {
        ShowUI(true);
    }
    public override void OnDeselect()
    {
        ShowUI(false);
    }

    public override void Initialize(Faction faction, factionTypes factionType)
    {
        base.Initialize(faction, factionType);
        if (factionType != factionTypes.player)
        {
            ShowUI(false);
        }
    }
}
