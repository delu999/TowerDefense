using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] TextMeshProUGUI healthUI;

    private void OnGUI() {
        currencyUI.text = CurrencyManager.Instance.Balance.ToString();
        healthUI.text = BaseLife.Instance.GetPercentage().ToString();
    }
}