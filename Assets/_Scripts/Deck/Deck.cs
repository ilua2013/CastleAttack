using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> _commonDeckPrefabs;
    [SerializeField] private List<Card> _combatDeckPrefabs;

    private List<Card> _commonDeck = new List<Card>();
    private List<Card> _combatDeck = new List<Card>();

    public List<Card> CommonDeck => _commonDeck;
    public List<Card> CombatDeck => _combatDeck;

    private void Awake()
    {
        if (Saves.HasKey(SaveController.Params.CommonDeck))
            _commonDeckPrefabs = Saves.GetDeck(SaveController.Params.CommonDeck).ToList();

        if (Saves.HasKey(SaveController.Params.CombatDeck))
            _combatDeckPrefabs = Saves.GetDeck(SaveController.Params.CommonDeck).ToList();

        CreateDeck();
    }

    private void CreateDeck()
    {
        foreach (Card card in _commonDeckPrefabs)
        {
            Card newCard = Instantiate(card);
            newCard.Activate(true);

            _commonDeck.Add(newCard);
        }

        foreach (Card card in _combatDeckPrefabs)
        {
            Card newCard = Instantiate(card);
            newCard.Activate(true);

            _combatDeck.Add(newCard);
        }
    }

    public Card[] ShowRandomCards(int count)
    {
        if (count > _combatDeck.Count)
            throw new ArgumentOutOfRangeException($"{count} is more then cards in the Deck");

        List<Card> result = new List<Card>(count);

        for (int i = 0; i < count; i++)
        {
            Card card = _combatDeck[Random.Range(0, _combatDeck.Count)];

            if (result.Contains(card))
            {
                i--;
                continue;
            }

            result.Add(card);
        }

        return result.ToArray();
    }

    public void ReturnCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            card.transform.SetParent(transform);
            card.gameObject.SetActive(false);
        }
    }
}
