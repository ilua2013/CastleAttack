using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsRewarder : MonoBehaviour
{
    private const int RewardPerEnemy = 5;

    [SerializeField] private int _reward;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private CoinsWallet _coinsWallet;

    public int ReceivedReward { get; private set; }

    private void OnValidate()
    {
        if (_coinsWallet == null)
            _coinsWallet = FindObjectOfType<CoinsWallet>();

        if (_battleSystem == null)
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
        ReceivedReward = _reward;

        _coinsWallet.Add(ReceivedReward, 0);
    }

    private void OnFailed()
    {
        ReceivedReward = _reward / 2;
        _coinsWallet.Add(ReceivedReward, 0);
    }
}
