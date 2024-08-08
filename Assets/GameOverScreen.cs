using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public Button restartButton;
    public string originalSceneName = "SampleScene";

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
    }

    void RestartGame()
    {
        Time.timeScale = 1;
        
        if (BaseLife.main is not null)
        {
            Destroy(BaseLife.main.gameObject);
            BaseLife.main = null;
        }
        
        SceneManager.LoadScene(originalSceneName);
    }
}