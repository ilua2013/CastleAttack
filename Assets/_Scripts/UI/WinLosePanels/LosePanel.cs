using Agava.YandexMetrica;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(FinishPanel))]
public class LosePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _coins;
    [SerializeField] private RewardRespawnButton _respawnButton;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private Rewarder _coinsRewarder;

    private FinishPanel _finishPanel;
    private UnitFriend _wizzard;

    private bool _isReviveWasOffered;

    private void OnValidate()
    {
        if (_battleSystem == null)
            _battleSystem = FindObjectOfType<BattleSystem>();

        if (_coinsRewarder == null)
            _coinsRewarder = FindObjectOfType<Rewarder>();
    }

    private void Awake()
    {
        _finishPanel = GetComponent<FinishPanel>();
        _wizzard = _battleSystem.Wizzard;
    }

    private void OnEnable()
    {
        _respawnButton.Respawned += OnRespawned;
        _finishPanel.Opened += OnPanelOpened;
        _wizzard.Fighter.ReadyToDie += OnPanelOpened;
    }

    private void OnDisable()
    {
        _respawnButton.Respawned -= OnRespawned;
        _finishPanel.Opened -= OnPanelOpened;
        _wizzard.Fighter.ReadyToDie -= OnPanelOpened;
    }

    private IEnumerator AnimateCoins(int award, Action onComplete = null)
    {
        float coins = 0;

        while (coins < award)
        {
            coins = Mathf.MoveTowards(coins, award, 1f);
            _coins.text = FormatCost(coins);

            yield return null;
        }

        _coins.text = FormatCost(award);
        onComplete?.Invoke();
    }

    private void OnPanelOpened()
    {
        _battleSystem.StopDoStep();
        _finishPanel.OpenPanel();

        StartCoroutine(AnimateCoins(_coinsRewarder.ReceivedCoins));

        if (_isReviveWasOffered == false)
            _isReviveWasOffered = true;
        else
            return;

#if !UNITY_EDITOR
        YandexMetrica.Send("ReviveAdOffer");
#endif
    }

    private void OnRespawned()
    {
        _battleSystem.ContinueDoStep();
        _wizzard.Fighter.RecoveryHealth(_battleSystem.Wizzard.Fighter.MaxHealth);
        _finishPanel.ClosePanel();

#if !UNITY_EDITOR
        YandexMetrica.Send("ReviveAdClick");
#endif
    }

    private string FormatCost(float cost)
    {
        var format = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        format.NumberGroupSeparator = " ";
        format.NumberDecimalDigits = 0;

        return Convert.ToDecimal(cost).ToString("N", format);
    }
}
