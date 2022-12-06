using Agava.YandexMetrica;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FinishPanel))]
public class LosePanel : MonoBehaviour
{
    [SerializeField] private RewardRespawnButton _respawnButton;
    [SerializeField] private BattleSystem _battleSystem;

    private FinishPanel _finishPanel;

    private bool _isReviveWasOffered;

    private void OnValidate()
    {
        _battleSystem = FindObjectOfType<BattleSystem>();
    }

    private void Awake()
    {
        _finishPanel = GetComponent<FinishPanel>();
    }

    private void OnEnable()
    {
        _respawnButton.Respawned += OnRespawned;
        _finishPanel.Opened += OnPanelOpened;
    }

    private void OnDisable()
    {
        _respawnButton.Respawned -= OnRespawned;
        _finishPanel.Opened -= OnPanelOpened;
    }

    private void OnPanelOpened()
    {
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
        _battleSystem.Wizzard.Fighter.RecoveryHealth(_battleSystem.Wizzard.Fighter.MaxHealth);
        _finishPanel.ClosePanel();

#if !UNITY_EDITOR
        YandexMetrica.Send("ReviveAdClick");
#endif
    }
}
