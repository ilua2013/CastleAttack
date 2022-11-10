using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeteorCardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private MeteorSpell _description;

    private SpellCard _card;
    private string _descriptionTextBase;

    private void Awake()
    {
        _card = GetComponent<SpellCard>();
        _descriptionTextBase = _text.text;
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
        _text.text = $"{_descriptionTextBase} {_description.Damage}";
        _amountText.text = $"{_card.Amount}";
    }
}
