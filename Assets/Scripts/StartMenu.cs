using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startButton.Select();
    }

    public void LoadStartScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadTutorialScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialMenu");
    }

    public void LoadStartMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartMenu");
    }
}
