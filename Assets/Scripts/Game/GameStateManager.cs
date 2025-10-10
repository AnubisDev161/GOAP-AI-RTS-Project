using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private int gameWonScreenSceneIndex;
    [SerializeField] private int gameOverScreenSceneIndex;
    [SerializeField] private int mainMenuSceneIndex;

    public void PlayerWon()
    {
        SceneManager.LoadScene(gameWonScreenSceneIndex);
    }

    public void PlayerLost()
    {
        SceneManager.LoadScene(gameOverScreenSceneIndex);
    }

    public void OnBackToMainMenuButtonClicked()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}

