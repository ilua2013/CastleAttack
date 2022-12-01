using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FinishPanel))]
public class LosePanel : MonoBehaviour
{
    [SerializeField] private RewardRespawnButton _respawnButton;

    private FinishPanel _finishPanel;
    private LevelSystem _levelSystem;

    private void Awake()
    {
        _levelSystem = FindObjectOfType<LevelSystem>();
        _finishPanel = GetComponent<FinishPanel>();
    }

    private void OnEnable()
    {
        _respawnButton.Respawned += OnRespawned;
    }

    private void OnDisable()
    {
        _respawnButton.Respawned -= OnRespawned;
    }

    private void OnRespawned()
    {
        _levelSystem.Wizzard.Fighter.RecoveryHealth(_levelSystem.Wizzard.Fighter.MaxHealth);
        _finishPanel.ClosePanel();
    }
}
