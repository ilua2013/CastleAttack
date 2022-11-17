using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CardSave
{
    [SerializeField] private bool _isAvailable;
    [SerializeField] private DeckType _deck;

    public CardSave(bool isAvailable, DeckType deck)
    {
        _isAvailable = isAvailable;
        _deck = deck;
    }

    public bool IsAvailable => _isAvailable;
    public DeckType Deck => _deck;
}
