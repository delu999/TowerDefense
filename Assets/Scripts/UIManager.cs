using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI waveUI;
    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] TextMeshProUGUI selectedTurretDescriptionUI;

    private void OnGUI() {
        if(currencyUI) currencyUI.text = CurrencyManager.Instance?.Balance.ToString();
        if(waveUI) waveUI.text = EnemySpawner.Instance?.CurrentWave.ToString();
        if(healthUI) healthUI.text = BaseLife.Instance?.GetPercentage().ToString();
        if(selectedTurretDescriptionUI) selectedTurretDescriptionUI.text = PlayerInput.Instance?.GetSelectedItemDecription();
    }
}