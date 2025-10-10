using UnityEngine;
using UnityEngine.SceneManagement;

public class factionDestroyedScreen : MonoBehaviour
{
    [SerializeField] private int mainMenuSceneIndex;

    public void OnBackToMainMenuButtonClicked()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }
}
