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
        Time.timeScale = 1;

        if (BaseLife.Instance is not null)
        {
            BaseLife.Instance.Restore();
        }
        
        if (EnemySpawner.Instance is not null)
        {
            EnemySpawner.Instance.Restore();
        }

        if (CurrencyManager.Instance is not null)
        {
            CurrencyManager.Instance.Restore();
        }
        
        if (PlayerInput.Instance is not null)
        {
            PlayerInput.Instance.Restore();
        }

        SceneManager.LoadScene(mainMenuSceneName);
    }
}