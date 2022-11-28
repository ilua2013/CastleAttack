using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FinishPanel))]
public class WinPanel : MonoBehaviour
{
    [SerializeField] private UnitFriend _wizzard;
    [SerializeField] private CardRewardPanel _cardRewardPanel;
    [SerializeField] private RewardStepsAnimation _stepsAnimation;

    private CardsRewarder _levelRewarder;
    private FinishPanel _finishPanel;

    private void Awake()
    {
        _levelRewarder = FindObjectOfType<CardsRewarder>();
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

        int starsCount = CalculateStarsCount(_wizzard.Fighter.RemainingHealth);

        _stepsAnimation.Play(starsCount);
    }

    private int CalculateStarsCount(float remain)
    {
        if (remain >= 0.5f && remain < 1f)
            return 2;
        else if (remain < 0.5f)
            return 1;
        else
            return 3;
    }
}
