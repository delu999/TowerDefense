using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] public int statingCurrency = 100;
    
    public static CurrencyManager Instance;

    public int Balance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Balance = statingCurrency;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCurrency(int amount)
    {
        Instance.Balance += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (Instance.Balance >= amount)
        {
            Instance.Balance -= amount;
            return true;
        }
        return false;
    }
}