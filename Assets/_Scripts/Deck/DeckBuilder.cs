using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField] private List<Card> _deckPrefabs;

    [SerializeField] private CommonDeck _commonDeck;
    [SerializeField] private CombatDeck _combatDeck;

    private void Awake()
    {
        foreach (Card prefab in _deckPrefabs)
        {
            Card card = Create(prefab);

            if (card.Deck is CombatDeck)
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
