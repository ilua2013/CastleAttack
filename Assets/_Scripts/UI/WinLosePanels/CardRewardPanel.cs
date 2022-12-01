using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardPanel : MonoBehaviour
{
    [SerializeField] private CardRewardView _containerPrefab;
    [SerializeField] private RewardIncreaseButton _rewardIncrease;
    [SerializeField] private Transform _panel;

    private List<Card> _cards = new List<Card>();
    private List<CardRewardView> _views = new List<CardRewardView>();

    private void OnEnable()
    {
        _rewardIncrease.Rewarded += OnRewardIncrease;
    }

    private void OnDisable()
    {
        _rewardIncrease.Rewarded -= OnRewardIncrease;
    }

    public void ShowCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            Card newCard = Instantiate(card);
            CardRewardView view = Instantiate(_containerPrefab, _panel);

            view.Fill(newCard, card.CardSave.RewardAmount);

            _views.Add(view);
            _cards.Add(newCard);
        }
    }

    private void OnRewardIncrease(int factor)
    {
        for (int i = 0; i < _views.Count; i++)
        {
            _views[i].Fill(_cards[i], _cards[i].CardSave.RewardAmount * factor);
        }
    }
}
