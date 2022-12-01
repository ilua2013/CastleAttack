using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CardSave
{
    [SerializeField] private bool _isAvailable = false;
    [SerializeField] private int _level = 1;
    [SerializeField] private int _amount = 0;
    [SerializeField] private int _rewardAmount = 0;
    [SerializeField] private int _amountToImprove = 3;
    [SerializeField] private UnitStats _unitStats;
    [SerializeField] private DeckType _deck = DeckType.Common;

    public CardSave(bool isAvailable = false, DeckType deck = DeckType.Common, int level = 1, int amount = 1, int amountToImprove = 3, int rewardAmount = 0)
    {
        _isAvailable = isAvailable;
        _deck = deck;
        _level = level;
        _amount = amount;
        _amountToImprove = amountToImprove;
        _rewardAmount = rewardAmount;
    }

    public bool IsAvailable => _isAvailable;
    public DeckType Deck => _deck;
    public int Level => _level;
    public int Amount => _amount;
    public int RewardAmount => _rewardAmount;
    public int AmountToImprove => _amountToImprove;
    public bool CanLevelUp => _amount >= _amountToImprove;
    public UnitStats UnitStats => _unitStats;

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

        _rewardAmount = amount;
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

    public override string ToString()
    {
        return $"IsAbailable - {IsAvailable}; Level - {Level}; Amount - {Amount}; AmountToImprove - {AmountToImprove}; Deck - {Deck}";
    }
}
