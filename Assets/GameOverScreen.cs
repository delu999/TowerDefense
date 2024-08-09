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
        
        if (BaseLife.Instance is not null)
        {
            Destroy(BaseLife.Instance.gameObject);
            // BaseLife.Instance = null; game over (TODO da sistemare facendo il reset)
        }
        
        SceneManager.LoadScene(originalSceneName);
    }
}