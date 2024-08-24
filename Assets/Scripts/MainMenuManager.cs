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
            EnemySpawner.Instance.StopAllCoroutines();
            Destroy(EnemySpawner.Instance.gameObject);
            EnemySpawner.Instance.Restore();
        }
        
        SceneManager.LoadScene(levelSceneBaseName+level);
    }
}