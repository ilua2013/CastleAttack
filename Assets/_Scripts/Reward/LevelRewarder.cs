using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class LevelRewarder : MonoBehaviour
{
    private LevelSystem _levelSystem;
    private DeckBuilder _deckBuilder;

    private Card[] _rewardCards;

    public Card[] RewardCards => _rewardCards;

    private void Awake()
    {
        _levelSystem = FindObjectOfType<LevelSystem>();
        _deckBuilder = FindObjectOfType<DeckBuilder>();
    }

    private void OnEnable()
    {
        _levelSystem.WaveFinished += OnFinished;
    }

    private void OnDisable()
    {
        _levelSystem.WaveFinished -= OnFinished;
    }

    private void OnFinished()
    {
        _rewardCards = GetRandomCards();

        foreach (Card card in _rewardCards)
        {
            SetRandomAmount(card);
            card.Save();
        }

        foreach (Card card in _rewardCards)
            Debug.Log($"{card.Name} is {card.CardSave.Amount}");
    }

    private Card[] GetRandomCards()
    {
        int count = UnityEngine.Random.Range(1, 4);
        Card[] cards = new Card[count];

        System.Random random = new System.Random();
        int[] index = Enumerable.Range(0, _deckBuilder.Cards.Count).OrderBy(t => random.Next()).Take(count).ToArray();

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i] = _deckBuilder.Cards[index[i]];
            cards[i].CardSave.SetAvailable(true);
        }

        return cards;
    }

    private void SetRandomAmount(Card card)
    {
        int amount = UnityEngine.Random.Range(1, 4);

        card.CardSave.Add(amount);
    }
}
