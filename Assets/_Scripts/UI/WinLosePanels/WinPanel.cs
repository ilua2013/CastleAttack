using Agava.YandexMetrica;
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

    private CellWinAnimations _cellWinAnimations;
    private DemolishAnimations _demolishAnimations;
    private CardsRewarder _levelRewarder;
    private FinishPanel _finishPanel;
    private bool _isRewardWasOffered;

    private void OnValidate()
    {
        foreach (var item in FindObjectsOfType<UnitFriend>())
            if (item.Fighter.FighterType == FighterType.MainWizzard)
                _wizzard = item;
    }

    private void Awake()
    {
        _levelRewarder = FindObjectOfType<CardsRewarder>();
        _cellWinAnimations = FindObjectOfType<CellWinAnimations>();
        _demolishAnimations = FindObjectOfType<DemolishAnimations>();
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

        Saves.SetInt(SaveController.Params.Level, SceneManager.GetActiveScene().buildIndex);
        Saves.Save();

        _increaseButton.Init(_levelRewarder.RewardCards);

        _cellWinAnimations.Play(() => 
        _demolishAnimations.Play(() => 
        {
            _finishPanel.OpenPanel();
            _stepsAnimation.Play(starsCount);
        }));

        if (_isRewardWasOffered == false)
            _isRewardWasOffered = true;
        else
            return;

#if !UNITY_EDITOR
        YandexMetrica.Send("RewardAdOffer");
#endif
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
