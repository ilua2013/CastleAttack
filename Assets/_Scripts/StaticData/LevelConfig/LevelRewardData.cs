using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelReward", menuName = "Configs/Level/Reward")]
public class LevelRewardData : ScriptableObject
{
    private const int StartCoins = 50;
    private const int CoinsStep = 50;

    [Header("Clear config")]
    public bool ClearAll;

    [Space]
    public int LevelCount;
    public List<RewardData> CardRewardData = new List<RewardData>();

    private void OnValidate()
    {
        if (ClearAll)
        {
            CardRewardData.Clear();

            for (int i = 0; i < LevelCount; i++)
                CardRewardData.Add(new RewardData(i, 1, StartCoins + CoinsStep * i));

            ClearAll = false;
        }
    }

    public RewardData GetAward(int level)
    {
        foreach (RewardData data in CardRewardData)
            if (data.Level == level)
                return data;

        return null;
    }
}


[Serializable]
public class RewardData
{
    public int Level;
    public Card Card;
    public int Amount;
    public int Coins;

    public RewardData(int level, int amount, int coins)
    {
        Level = level;
        Amount = amount;
        Coins = coins;
    }
}