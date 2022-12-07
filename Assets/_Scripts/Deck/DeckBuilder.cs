using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField] private List<Card> _deckPrefabs;
    [SerializeField] private CommonDeck _commonDeck;
    [SerializeField] private CombatDeck _combatDeck;

    private List<Card> _cards = new List<Card>();

    public List<Card> Cards => _cards;

    private void OnValidate()
    {
        _commonDeck = FindObjectOfType<CommonDeck>();
    }

    private void Awake()
    {
        foreach (Card prefab in _deckPrefabs)
        {
            Card card = Create(prefab);
            _combatDeck.Add(card);

            for (int i = 0; i < card.CardSave.Amount - 1; i++)
                _combatDeck.Add(Create(prefab));

            if (card.CardSave.IsAvailable == false)
                continue;

            _commonDeck.Add(Create(prefab));
        }
    }

    public Card TakeCard(CardName name)
    {
        foreach (Card card in Cards)
            if (card.Name == name)
            {
                _commonDeck.Add(card);
                return card;
            }

        throw new ArgumentException($"This card {name} does not exist");
    }

    private Card Create(Card prefab)
    {
        Card card = Instantiate(prefab);

        card.Activate(true);
        card.Load();

        return card;
    }
}
