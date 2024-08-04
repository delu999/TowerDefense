using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseLife : MonoBehaviour
{
    public int startingLife = 20;
    private int _currentLife;

    public static BaseLife main;
    
    private void Awake()
    {
        if (main is null)
        {
            _currentLife = startingLife;
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DecreaseLife(int amount = 1)
    {
        _currentLife -= amount;
        if (_currentLife <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;
        SceneManager.LoadScene("GameOverScene");
    }
}