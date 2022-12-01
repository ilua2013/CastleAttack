using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinsWallet : MonoBehaviour
{
    public int Coins { get; private set; }

    public event Action<int> CoinsChanged;

    private void Awake()
    {
        if (Saves.HasKey(SaveController.Params.Coins))
            Coins = Saves.GetInt(SaveController.Params.Coins);
    }

    public bool TrySpend(int amount)
    {
        int estimated = Coins - amount;

        if (estimated < 0)
            return false;

        Coins = estimated;
        Saves.SetInt(SaveController.Params.Coins, Coins);
        Saves.Save();
        CoinsChanged?.Invoke(amount);

        return true;
    }

    public void Add(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        Coins += amount;
        Saves.SetInt(SaveController.Params.Coins, Coins);
        Saves.Save();
        CoinsChanged?.Invoke(amount);
    }
}
