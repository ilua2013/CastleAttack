using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatDeck : Deck
{
    public override DeckType DeckType => DeckType.Combat;

    public Card[] ShowRandomCards(int count)
    {
        if (count > Cards.Count)
            throw new ArgumentOutOfRangeException($"{count} is more then cards in the Deck");

        List<Card> result = new List<Card>(count);

        for (int i = 0; i < count; i++)
        {
            Card card = Cards[Random.Range(0, Cards.Count)];

            if (result.Contains(card))
            {
                i--;
                continue;
            }

            result.Add(card);
            Cards.Remove(card);
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
