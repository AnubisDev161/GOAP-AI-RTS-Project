using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEventListener : MonoBehaviour
{
    private void OnEnable()
    {
        Townhall.FactionDestroyed += OnFactionDestroyed;
    }

    private void OnDisable()
    {
        Townhall.FactionDestroyed -= OnFactionDestroyed;
    }

    // just some simple code to react to the destruction of a faction
    public void OnFactionDestroyed(Building.factionTypes factionType)
    {
        if(factionType == Building.factionTypes.player)
        {
            GetComponentInParent<GameStateManager>().PlayerLost();
        }
        else if (factionType == Building.factionTypes.CPU)
        {
            GetComponentInParent<GameStateManager>().PlayerWon();
        }
    }
}
