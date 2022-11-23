using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField] private List<Card> _deckPrefabs;

    [SerializeField] private CommonDeck _commonDeck;
    [SerializeField] private CombatDeck _combatDeck;

    private List<Card> _cards = new List<Card>();

    public List<Card> Cards => _cards;

    private void Awake()
    {
        foreach (Card prefab in _deckPrefabs)
        {
            Card card = Create(prefab);
            _cards.Add(card);

            Debug.Log($"{card.Name} in amount of {card.CardSave.Amount} isAvailable {card.CardSave.IsAvailable}");

            if (card.CardSave.IsAvailable == false)
                continue;

            if (card.CardSave.Deck == DeckType.Combat)
                _combatDeck.Add(card);
            else
                _commonDeck.Add(card);
        }
    }

    private Card Create(Card prefab)
    {
        Card card = Instantiate(prefab);

        card.Activate(true);
        card.Load();

        return card;
    }
}
