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

    private void Awake()
    {
        _card = GetComponent<Card>();
    }

    private void OnEnable()
    {
        _card.Used += OnCardUse;
    }

    private void OnDisable()
    {
        _card.Used -= OnCardUse;
    }

    private void Start()
    {
        WriteText();
    }

    private void OnCardUse(int amount)
    {
        WriteText();
    }

    private void WriteText()
    {
        _text.text = $"{_fighter.Damage} {_text.text}"; // Требует корректировки
        _amountText.text = $"{_card.Amount}";
    }
}
