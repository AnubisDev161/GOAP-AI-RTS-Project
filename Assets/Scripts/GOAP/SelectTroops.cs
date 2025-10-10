internal class SelectTroops : IActionStrategy
{
    readonly IGoapInteractor goapInteractor;

    public bool canPerform => !complete;
    public bool complete => goapInteractor.HasSelectedTroops() == true;

    public SelectTroops(IGoapInteractor goapInteractor)
    {
        this.goapInteractor = goapInteractor;
    }

    public void Start()
    {
        goapInteractor.SelectTroops();
    }
}