using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform _combatDeckPanel;
    [SerializeField] private Transform _commonDeckPanel;

    private Deck _deck;

    private List<Card> _commonDeck = new List<Card>();
    private List<Card> _combatDeck = new List<Card>();

    private void Awake()
    {
        _deck = FindObjectOfType<Deck>();
    }

    private void OnEnable()
    {
        _commonDeck = _deck.CommonDeck;
        foreach (Card card in _deck.CommonDeck)
        {
            card.BeginDrag += OnBeginDrag;
            card.EndDrag += OnEndDrag;
        }

        foreach (Card card in _deck.C)
        {
            card.BeginDrag += OnBeginDrag;
            card.EndDrag += OnEndDrag;
        }
    }

    private void OnDisable()
    {
        foreach (Card card in _commonDeck)
        {
            card.BeginDrag -= OnBeginDrag;
            card.EndDrag -= OnEndDrag;
        }

        foreach (Card card in _combatDeck)
        {
            card.BeginDrag -= OnBeginDrag;
            card.EndDrag -= OnEndDrag;
        }
    }

    private void CreateDeck()
    {
        foreach (Card card in _commonDeckPrefabs)
        {
            Card newCard = Instantiate(card);
            newCard.Activate(true);

            _commonDeck.Add(newCard);
        }

        foreach (Card card in _combatDeckPrefabs)
        {
            Card newCard = Instantiate(card);
            newCard.Activate(true);

            _combatDeck.Add(newCard);
        }
    }

    private bool CanTransfer(PointerEventData eventData, Card card)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var item in results)
        {
            if (!item.isValid)
                continue;

            if (item.gameObject.transform == _combatDeckPanel)
                return true;
        }

        return false;
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        card.transform.SetParent(transform);
    }
    
    private void OnEndDrag(PointerEventData eventData, Card card)
    {
        if (CanTransfer(eventData, card))
            
    }
}
