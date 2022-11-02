using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Card))]
public class UnitCardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Fighter _fighter;

    private Card _card;
    private string _descriptionTextBase;

    private void Awake()
    {
        _card = GetComponent<Card>();
        _descriptionTextBase = _text.text;
        WriteText();
    }

    private void OnEnable()
    {
        _card.Used += OnCardUse;
    }

    private void OnDisable()
    {
        _card.Used -= OnCardUse;
    }

    private void OnCardUse(int amount)
    {
        WriteText();
    }

    private void WriteText()
    {
        _text.text = $"{_descriptionTextBase} {_fighter.Damage}"; // Требует корректировки
        _amountText.text = $"{_card.Amount}";
    }
}
