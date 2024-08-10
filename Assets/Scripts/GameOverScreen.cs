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
            BaseLife.Instance.StopAllCoroutines();
            Destroy(BaseLife.Instance.gameObject);
            BaseLife.Instance.Restore();
        }
        
        if (EnemySpawner.Instance is not null)
        {
            EnemySpawner.Instance.StopAllCoroutines();
            Destroy(EnemySpawner.Instance.gameObject);
            EnemySpawner.Instance.Restore();
        }
        
        SceneManager.LoadScene(originalSceneName);
    }
}