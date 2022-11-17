using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DeckType
{
    Common,
    Combat,
}

public class Deck : MonoBehaviour
{
    private List<Card> _cards = new List<Card>();

    public List<Card> Cards => _cards;
    public virtual DeckType DeckType { get; }

    public void Add(Card card)
    {
        _cards.Add(card);
        card.Save(DeckType);
    }

    public void Remove(Card card)
    {
        _cards.Remove(card);
    }
}
