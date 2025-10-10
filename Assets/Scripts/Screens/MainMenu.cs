using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int gameSceneIndex = 1;
    [SerializeField] private int credtisSceneIndex = 4;
    public void OnQuitButtonClicked()
    {
    
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }


    public void OnCreditsbuttonClicked()
    {
        SceneManager.LoadScene(credtisSceneIndex);
    }

    public void OnNewGameButtonClicked()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }
}
