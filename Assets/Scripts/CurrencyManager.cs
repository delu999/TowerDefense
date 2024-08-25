using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] public int statingCurrency = 50;
    
    public static CurrencyManager Instance { get; private set; }
    public int Balance { get; private set; }
    
    private void Awake()
    {
        if (Instance is not null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Balance = statingCurrency;
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
    }

    public void AddCurrency(int amount)
    {
        Balance += amount;
    }

    public bool CanSpendCurrency(int amount)
    {
        return Balance >= amount;
    }
    
    public void SpendCurrency(int amount)
    {
        if (CanSpendCurrency(amount))
        {
            Balance -= amount;
        }
    }

    public void Restore() {
        Instance = null;
    }
}