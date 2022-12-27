using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField] private List<Card> _deckPrefabs;
    
    private CommonDeck _commonDeck;
    private CombatDeck _combatDeck;
    private List<Card> _cards = new List<Card>();

    public List<Card> Cards => _cards;

    private void Awake()
    {
        _commonDeck = GetComponentInChildren<CommonDeck>();
        _combatDeck = GetComponentInChildren<CombatDeck>();

        foreach (Card prefab in _deckPrefabs)
        {
            Card card = Create(prefab);

            if (card.CardSave.IsAvailable)
            {
                _combatDeck.Add(card);

                for (int i = 0; i < card.CardSave.Amount - 1; i++)
                    _combatDeck.Add(Create(prefab));
            }

            _commonDeck.Add(Create(prefab));
        }
    }

    public Card GetCard(CardName name)
    {
        foreach (Card card in _commonDeck.Cards)
            if (card.Name == name)
                return card;

        throw new InvalidOperationException("No such card exists");
    }

    private Card Create(Card prefab)
    {
        Card card = Instantiate(prefab);

        card.gameObject.SetActive(false);
        card.Activate(true);
        card.Load();

        return card;
    }
}
