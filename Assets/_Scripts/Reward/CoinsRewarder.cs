using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsRewarder : MonoBehaviour
{
    private const int Reward = 100;
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
        ReceivedReward = Reward;

        _coinsWallet.Add(ReceivedReward, 0);
    }

    private void OnFailed()
    {
        ReceivedReward = Reward / 2;

        _coinsWallet.Add(ReceivedReward / 2, 0);
    }
}
