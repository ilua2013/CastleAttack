using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardCostView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _maxCardsText;

    private CardInShopView _cardInShopView;

    private void Awake()
    {
        _cardInShopView = GetComponentInParent<CardInShopView>();
    }

    private void OnEnable()
    {
        _cardInShopView.CostUpdated += OnCostUpdate;
        _cardInShopView.CardFull += OnCardFull;
    }

    private void OnDisable()
    {
        _cardInShopView.CostUpdated -= OnCostUpdate;
        _cardInShopView.CardFull -= OnCardFull;
    }

    private void OnCardFull(bool isFull)
    {
        _maxCardsText.enabled = isFull;
        _text.enabled = !isFull;
    }

    private void OnCostUpdate(int cost)
    {
        _text.text = cost.ToString();
    }
}
