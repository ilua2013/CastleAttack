using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsRewarder : MonoBehaviour
{
    private const int Reward = 50;
    private const int RewardPerEnemy = 5;

    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private CoinsWallet _coinsWallet;

    public int ReceivedReward { get; private set; }

    private void OnValidate()
    {
        _coinsWallet = FindObjectOfType<CoinsWallet>();
        _battleSystem = FindObjectOfType<BattleSystem>();
    }

    private void OnEnable()
    {
        _battleSystem.Win += OnFinished;
        _battleSystem.Lose += OnFailed;
    }

    private void OnDisable()
    {
        _battleSystem.Win -= OnFinished;
        _battleSystem.Lose -= OnFailed;
    }

    private void OnFinished()
    {
        ReceivedReward = Reward + GamesStatistics.KilledEnemy * RewardPerEnemy;

        _coinsWallet.Add(ReceivedReward, 0);
    }

    private void OnFailed()
    {
        ReceivedReward = (Reward + GamesStatistics.KilledEnemy * RewardPerEnemy) / 2;

        _coinsWallet.Add(ReceivedReward / 2, 0);
    }
}
