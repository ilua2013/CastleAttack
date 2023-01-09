using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PanelStartLevelView : MonoBehaviour
{
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private CoinsAwardCard _coinsCard;
    [SerializeField] private Transform _panel;
    [SerializeField] private CardRewardView _rewardViewPrefab;
    [SerializeField] private LevelRewardData _levelRewardData;

    private string _levelTextInitial;
    private List<CardRewardView> _rewardViews = new List<CardRewardView>();

    private void Awake()
    {
        _levelTextInitial = _levelText.text;
    }

    public void Init(int level)
    {
        RewardData rewardData = _levelRewardData.GetAward(level);

        if (rewardData.Card != null)
            FillCardReward(rewardData);
        else
            ClearCardReward();

        FillTexts(level, rewardData.Coins);
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
        CardRewardView view = Instantiate(_rewardViewPrefab, _panel);

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
