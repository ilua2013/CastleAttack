using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CardSave
{
    [SerializeField] private bool _isAvailable;
    [SerializeField] private Deck _deck;

    public CardSave(bool isAvailable, Deck deck)
    {
        _isAvailable = isAvailable;
        _deck = deck;
    }

    public bool IsAvailable => _isAvailable;
    public Deck Deck => _deck;
}
