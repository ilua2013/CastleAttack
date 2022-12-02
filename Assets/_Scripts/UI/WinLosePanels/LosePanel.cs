using Agava.YandexMetrica;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FinishPanel))]
public class LosePanel : MonoBehaviour
{
    [SerializeField] private RewardRespawnButton _respawnButton;

    private FinishPanel _finishPanel;
    private LevelSystem _levelSystem;

    private bool _isReviveWasOffered;

    private void Awake()
    {
        _levelSystem = FindObjectOfType<LevelSystem>();
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

        YandexMetrica.Send("ReviveAdOffer");
    }

    private void OnRespawned()
    {
        _levelSystem.Wizzard.Fighter.RecoveryHealth(_levelSystem.Wizzard.Fighter.MaxHealth);
        _finishPanel.ClosePanel();

        YandexMetrica.Send("ReviveAdClick");
    }
}
