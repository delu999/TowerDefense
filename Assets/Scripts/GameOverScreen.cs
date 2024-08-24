using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Button mainMenuButton;
    public string mainMenuSceneName = "MainMenuScene";

    void Start()
    {
        mainMenuButton.onClick.AddListener(ReturnToMainScreen);
    }
    
    void ReturnToMainScreen() {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}