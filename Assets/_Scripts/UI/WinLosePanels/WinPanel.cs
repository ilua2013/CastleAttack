using Agava.YandexMetrica;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FinishPanel))]
public class WinPanel : MonoBehaviour
{
    [SerializeField] private UnitFriend _wizzard;
    [SerializeField] private CardRewardPanel _cardRewardPanel;
    [SerializeField] private RewardIncreaseButton _increaseButton;
    [SerializeField] private RewardStepsAnimation _stepsAnimation;
    [SerializeField] private DemolishAnimations _demolishAnimations;
    [SerializeField] private CellWinAnimations _cellWinAnimations;
    [SerializeField] private Rewarder _levelRewarder;

    private FinishPanel _finishPanel;
    private bool _isRewardWasOffered;

    private void OnValidate()
    {
        if (_demolishAnimations == null)
            _demolishAnimations = FindObjectOfType<DemolishAnimations>(true);

        if (_cellWinAnimations == null)
            _cellWinAnimations = FindObjectOfType<CellWinAnimations>(true);

        if (_levelRewarder == null)
            _levelRewarder = FindObjectOfType<Rewarder>(true);

        foreach (var item in FindObjectsOfType<UnitFriend>())
            if (item.Fighter.FighterType == FighterType.MainWizzard)
                _wizzard = item;
    }

    private void Awake()
    {
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
        int starsCount = CalculateStarsCount(_wizzard.Fighter.RemainingHealth);

        _increaseButton.Init(_levelRewarder.RewardCards);
        _levelRewarder.Init();

        SaveProgress();

        PlayWinAnimation(starsCount);

        if (_isRewardWasOffered == false)
            _isRewardWasOffered = true;
        else
            return;
    }

    private void PlayWinAnimation(int starsCount)
    {
        _cellWinAnimations.Play(() =>
        _demolishAnimations.Play(() =>
        {
            _finishPanel.OpenPanel();
            _stepsAnimation.Play(starsCount);
        }));
    }

    private void SaveProgress()
    {
        if (SceneManager.GetActiveScene().buildIndex != SceneLoader.TutorialIndex)
        {
            if (Saves.HasKey(SaveController.Params.CompletedLevel))
                if (Saves.GetInt(SaveController.Params.CompletedLevel) > Saves.SelectedLevel) // ���� �������� ��� ������ � ������ ���� ��� ��� ��������
                    return;

            Saves.SetInt(SaveController.Params.CompletedLevel, Saves.SelectedLevel);
            Saves.Save();
        }
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
