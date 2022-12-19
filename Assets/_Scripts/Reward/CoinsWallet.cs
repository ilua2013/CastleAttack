using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinsWallet : MonoBehaviour
{
    public int Coins { get; private set; }

    public event Action<int, float> CoinsChanged;

    private void Awake()
    {
        if (Saves.HasKey(SaveController.Params.Coins))
            Coins = Saves.GetInt(SaveController.Params.Coins);

        Debug.Log("Awwake" + Coins);
    }

    public bool TrySpend(int amount)
    {
        Debug.Log("TrySp[end");
        int estimated = Coins - amount;

        if (estimated < 0)
            return false;

        Coins = estimated;

        Saves.SetInt(SaveController.Params.Coins, Coins);
        Saves.Save();

        CoinsChanged?.Invoke(amount, 0);

        return true;
    }

    public void Add(int amount, float delay)
    {
        Debug.Log("Add");

        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        Coins += amount;

        Saves.SetInt(SaveController.Params.Coins, Coins);
        Saves.Save();

        CoinsChanged?.Invoke(amount, delay);
    }
}
