using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PanelStartLevelView : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [Header("Reward Panel")]
    [SerializeField] private CoinsAwardCard _coinsCard;
    [SerializeField] private Transform _rewardPanel;
    [SerializeField] private CardRewardView _rewardViewPrefab;
    [Header("Enemies Panel")]
    [SerializeField] private Transform _enemiesPanel;
    [SerializeField] private EnemyCardViewContainer _enemiesContainer;
    [Header("Level data")]
    [SerializeField] private LevelRewardData _levelRewardData;
    [SerializeField] private LevelEnemiesData _levelEnemiesData;

    private string _levelTextInitial;
    private List<CardRewardView> _rewardViews = new List<CardRewardView>();
    private List<EnemyCardView> _enemyCards = new List<EnemyCardView>();

    private void Awake()
    {
        _levelTextInitial = _levelText.text;
    }

    public void Init(int level)
    {
        RewardData rewardData = _levelRewardData.GetAward(level);
        EnemiesData enemiesData = _levelEnemiesData.GetEnemiesOnLevel(level);

        FillCards(rewardData);
        FillEnemies(enemiesData);

        FillTexts(level, rewardData.Coins);
    }

    private void FillEnemies(EnemiesData data)
    {
        foreach (EnemyCardView view in _enemyCards)
            view.gameObject.SetActive(false);

        _enemyCards.Clear();

        foreach (EnemyType type in data.EnemyTypes)
        {
            EnemyCardView view = _enemiesContainer.GetEnemyView(type.EnemyPrefab.TypeId);
            _enemyCards.Add(view);

            view.gameObject.SetActive(true);
            view.Init(type.Amount);
        }
    }

    private void FillCards(RewardData rewardData)
    {
        if (rewardData.Card != null)
            FillCardReward(rewardData);
        else
            ClearCardReward();
    }

    private void FillTexts(int level, int coins)
    {
        _levelText.text = $"{_levelTextInitial} {level}";
        _coinsCard.Init(coins);
    }

    private void ClearCardReward()
    {
        foreach (CardRewardView item in _rewardViews)
            item.Clear();
    }

    private void FillCardReward(RewardData rewardData)
    {
        CardRewardView cardReward = _rewardViews.FirstOrDefault((card) => card.Card != null && card.Card.Name == rewardData.Card.Name);
        ClearCardReward();

        if (cardReward != null)
            FillCardView(rewardData, cardReward.Card, cardReward);
        else
            CreateNewCard(rewardData);
    }

    private void CreateNewCard(RewardData rewardData)
    {
        Card newCard = Instantiate(rewardData.Card);
        CardRewardView view = Instantiate(_rewardViewPrefab, _rewardPanel);

        FillCardView(rewardData, newCard, view);
    }

    private void FillCardView(RewardData rewardData, Card newCard, CardRewardView view)
    {
        view.Fill(newCard, rewardData.Amount);
        _rewardViews.Add(view);

        if (newCard.TryGetComponent(out CardAnonimusView anonimusView))
            anonimusView.ReskinToInitial();
    }
}
