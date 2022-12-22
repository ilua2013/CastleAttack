using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private CommonDeck _deck;
    private List<CardInShopView> _views;
    private List<Card> _cards = new List<Card>();

    public event Action<Card> CardBought;
    public event Action<Card> CardOpened;

    private void Awake()
    {
        _deck = FindObjectOfType<CommonDeck>();
        _views = GetComponentsInChildren<CardInShopView>().ToList();
    }

    private void OnEnable()
    {
        foreach (CardInShopView view in _views)
        {
            view.CardBought += OnCardBought;
            view.CardOpened += OnCardOpened;
        }
    }

    private void OnDisable()
    {
        foreach (CardInShopView view in _views)
        {
            view.CardBought -= OnCardBought;
            view.CardOpened -= OnCardOpened;
        }
    }

    private void Start()
    {
        foreach (Card card in _deck.Cards)
            _cards.Add(Instantiate(card));

        FillCards(false);
    }

    private void FillCards(bool smooth)
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            for (int j = 0; j < _views.Count; j++)
            {
                if (_cards[i].Name == _views[j].CardName)
                    _views[j].FillCard(_cards[i], false);
            }
        }
    }

    private void OnCardBought(Card card)
    {
        CardBought?.Invoke(card);
    }

    private void OnCardOpened(Card card)
    {
        _cards.Add(card);
        CardOpened?.Invoke(card);
    }
}
