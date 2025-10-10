internal class AssignTroopsToTargets : IActionStrategy
{
    readonly IGoapInteractor goapInteractor;
    Faction.factionTypes opponentFaction;
    public bool canPerform => !complete;
    public bool complete => goapInteractor.HasSelectedTroops() == true;

    public AssignTroopsToTargets(IGoapInteractor goapInteractor, Faction.factionTypes opponentFaction)
    {
        this.goapInteractor = goapInteractor;
        this.opponentFaction = opponentFaction;
    }

    public void Start()
    {
        goapInteractor.AsssignTroopsToTargets(goapInteractor.FindTarget());
    }
}