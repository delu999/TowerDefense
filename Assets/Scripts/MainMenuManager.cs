using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager: MonoBehaviour
{
    public string levelSceneBaseName = "Level_";

    public void StartLevel(int level)
    {
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
        
        SceneManager.LoadScene(levelSceneBaseName+level);
    }
}