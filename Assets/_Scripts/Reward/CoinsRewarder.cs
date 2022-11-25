using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsRewarder : MonoBehaviour
{
    private const int Reward = 50;
    private const int RewardPerEnemy = 5;

    private LevelSystem _levelSystem;
    private CoinsWallet _coinsWallet;

    public int ReceivedReward { get; private set; }

    private void Awake()
    {
        _coinsWallet = FindObjectOfType<CoinsWallet>();
        _levelSystem = FindObjectOfType<LevelSystem>();
    }

    private void OnEnable()
    {
        _levelSystem.Wave3Finished += OnFinished;
    }

    private void OnDisable()
    {
        _levelSystem.Wave3Finished -= OnFinished;
    }

    private void OnFinished()
    {
        ReceivedReward = Reward + GamesStatistics.KilledEnemy * RewardPerEnemy;

        _coinsWallet.Add(ReceivedReward);
    }
}
