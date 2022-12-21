using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CombatDeck : Deck
{
    public override DeckType DeckType => DeckType.Combat;

    private List<UnitCard> _unitCards = new List<UnitCard>();

    public event Action CardTaken;
    public event Action CardReturned;

    private void OnEnable()
    {
        CardAdded += OnCardAdd;
    }

    private void OnDisable()
    {
        foreach (UnitCard card in _unitCards)
            card.CameBack -= ReturnCard;
    }

    private void OnCardAdd(Card card)
    {
        if (card is UnitCard unitCard)
        {
            if (_unitCards.Contains(unitCard))
                return;

            _unitCards.Add(unitCard);
            unitCard.CameBack += ReturnCard;
        }
    }

    public Card[] TakeRandomCards(int count)
    {
        if (count > Cards.Count)
            count = Cards.Count;

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
            Remove(card);
            CardTaken?.Invoke();
        }

        return result.ToArray();
    }

    public void ReturnCard(Card card)
    {
        Debug.Log("Return");
        Add(card);
        CardReturned?.Invoke();
    }
}
