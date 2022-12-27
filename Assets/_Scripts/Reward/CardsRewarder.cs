using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

[Serializable]
public class CardReward
{
    [field: SerializeField] public CardName CardName { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
}

public class CardsRewarder : MonoBehaviour
{
    [SerializeField] private CardReward[] _rewards;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private DeckBuilder _deckBuilder;
    [SerializeField] private CardRewardPanel _cardRewardPanel;

    private Card[] _rewardCards;

    public Card[] RewardCards => _rewardCards;

    private void OnValidate()
    {
        if (_battleSystem == null)
            _battleSystem = FindObjectOfType<BattleSystem>();

        if (_deckBuilder == null)
            _deckBuilder = FindObjectOfType<DeckBuilder>();

        if (_cardRewardPanel == null)
            _cardRewardPanel = FindObjectOfType<CardRewardPanel>(true);
    }

    private void OnEnable()
    {
        _battleSystem.Win += OnFinished;
    }

    private void OnDisable()
    {
        _battleSystem.Win -= OnFinished;
    }

    private void OnFinished()
    {
        List<Card> cards = new List<Card>();

        foreach (CardReward reward in _rewards)
        {
            Card card = _deckBuilder.GetCard(reward.CardName);

            card.CardSave.SetAvailable(true);
            card.CardSave.SetDeck(DeckType.Combat);
            card.CardSave.Add(reward.Amount);
            card.Save();

            Debug.Log("Вы получили карту - " + card.Name + " в кол-ве " + card.Amount + " штук");

            cards.Add(card);
        }

        _cardRewardPanel.ShowCards(cards.ToArray());
    }
}
