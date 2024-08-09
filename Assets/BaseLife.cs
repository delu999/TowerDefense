using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseLife : MonoBehaviour
{
    public int startingLife = 20;
    public int CurrentLife { get; private set; }
    
    public static BaseLife Instance { get; private set; }

    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            CurrentLife = startingLife;
            Instance = this;
        }
    }

    public void DecreaseLife(int amount = 1)
    {
        CurrentLife -= amount;
        if (CurrentLife <= 0)
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

    public int GetPercentage()
    {
        return (int)((float)CurrentLife / startingLife * 100);
    }
}