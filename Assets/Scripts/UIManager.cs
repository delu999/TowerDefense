using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI waveUI;
    [SerializeField] TextMeshProUGUI waveTimerUI;
    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] TextMeshProUGUI selectedTurretDescriptionUI;
    [SerializeField] Button goToMenuButton;

    private void Start()
    {
        goToMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void OnGUI() {
        if(currencyUI) currencyUI.text = CurrencyManager.Instance?.Balance.ToString();
        if(waveUI) waveUI.text = EnemySpawner.Instance?.CurrentWave.ToString();
        if(waveTimerUI) waveTimerUI.text = ((int)EnemySpawner.Instance?.CountdownUntilNextWave)+"s";
        if(healthUI) healthUI.text = BaseLife.Instance?.GetPercentage().ToString();
        if(selectedTurretDescriptionUI) selectedTurretDescriptionUI.text = PlayerInput.Instance?.GetSelectedItemDecription();
    }

    private void GoToMainMenu()
    {
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

        SceneManager.LoadScene("MainMenuScene");
    }
}