using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealCardView : CardView
{
    private SpellCard _card;

    private void Awake()
    {
        _card = GetComponent<SpellCard>();
        WriteText();
    }

    private void OnEnable()
    {
        _card.Used += OnCardUse;
        _card.AmountChanged += OnAmountChange;
    }

    private void OnDisable()
    {
        _card.Used -= OnCardUse;
        _card.AmountChanged -= OnAmountChange;
    }

    private void OnAmountChange(int amount)
    {
        WriteText();
    }

    private void OnCardUse(int amount)
    {
        WriteText();
    }

    private void WriteText()
    {
        Text.text = $"{_card.CardSave.UnitStats.MaxHealth}";
        AmountText.text = $"{_card.Amount}";
    }
}
