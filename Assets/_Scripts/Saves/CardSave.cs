using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CardSave
{
    [SerializeField] private bool _isAvailable;
    [SerializeField] private int _level;
    [SerializeField] private int _amount;
    [SerializeField] private int _amountToImprove;
    [SerializeField] private DeckType _deck;

    public CardSave(bool isAvailable, DeckType deck, int level = 1, int amount = 1, int amountToImprove = 3)
    {
        _isAvailable = isAvailable;
        _deck = deck;
        _level = level;
        _amount = amount;
        _amountToImprove = amountToImprove;
    }

    public bool IsAvailable => _isAvailable;
    public DeckType Deck => _deck;
    public int Level => _level;
    public int Amount => _amount;
    public int AmountToImprove => _amountToImprove;
    public bool CanLevelUp => _amount >= _amountToImprove;

    public void SetAvailable(bool isAvailable)
    {
        _isAvailable = isAvailable;
    }

    public void SetDeck(DeckType deck)
    {
        _deck = deck;
    }

    public void Add(int amount)
    {
        if (amount < 0)
            throw new ArgumentException(nameof(amount));

        _amount += amount;
    }

    public void LevelUp()
    {
        if (CanLevelUp)
        {
            _level++;
            _amount -= _amountToImprove;
        }
    }
}
