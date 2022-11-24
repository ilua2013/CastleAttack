using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FinishPanel))]
public class WinPanel : MonoBehaviour
{
    [SerializeField] private CardRewardPanel _cardRewardPanel;

    private LevelRewarder _levelRewarder;
    private FinishPanel _finishPanel;

    private void Awake()
    {
        _levelRewarder = FindObjectOfType<LevelRewarder>();
        _finishPanel = GetComponent<FinishPanel>();
    }

    private void OnEnable()
    {
        _finishPanel.Opened += OnOpened;
    }

    private void OnDisable()
    {
        _finishPanel.Opened -= OnOpened;
    }

    private void OnOpened()
    {
        _cardRewardPanel.ShowCards(_levelRewarder.RewardCards);
    }
}
