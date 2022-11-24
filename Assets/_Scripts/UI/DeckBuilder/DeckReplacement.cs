using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckReplacement : MonoBehaviour
{
    [SerializeField] private Transform _dragging;
    [SerializeField] private Deck _deck;

    private List<CardInDeckView> _views;
    private List<CardMovement> _movements = new List<CardMovement>();
    private List<Card> _cards = new List<Card>();

    public bool CanTransfer => _views.Count > _cards.Count;

    private void Awake()
    {
        _views = GetComponentsInChildren<CardInDeckView>().ToList();
    }

    private void OnEnable()
    {
        _cards = new List<Card>(_deck.Cards);

        foreach (Card card in _cards)
            Register(card);

        FillCards(false);
    }

    private void OnDisable()
    {
        foreach (Card card in _cards)
            UnRegister(card);
    }

    private void Update()
    {
        //foreach (CardMovement movement in _movements)
        //    movement.Move();
    }

    public void AddCard(Card card)
    {
        Register(card);
        _cards.Add(card);
        _deck.Add(card);
        FillCards(true);
    }

    public void RemoveCard(Card card)
    {
        UnRegister(card);
        _cards.Remove(card);
        _deck.Remove(card);
        FillCards(true);
    }

    private void TryTransferCard(PointerEventData eventData, Card card)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var item in results)
        {
            if (!item.isValid)
                continue;

            if (item.gameObject.TryGetComponent(out DeckReplacement deckView))
            {
                if (deckView == this)
                {
                    FillCards(true);
                    return;
                }
                else
                {
                    if (deckView.CanTransfer)
                    {
                        deckView.AddCard(card);
                        RemoveCard(card);
                        return;
                    }
                }
            }
        }

        FillCards(true);
    }

    private void FillCards(bool smooth)
    {
        for (int i = 0; i < _cards.Count && i < _views.Count; i++)
        {
            _views[i].FillCard(_cards[i], smooth);
        }
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        card.transform.SetParent(_dragging.parent);
    }

    private void OnEndDrag(PointerEventData eventData, Card card)
    {
        TryTransferCard(eventData, card);
    }

    private void Register(Card card)
    {
        if (card.TryGetComponent(out CardMovement movement))
        {
            movement.Init(_dragging);
            _movements.Add(movement);
        }

        //card.BeginDrag += OnBeginDrag;
        //card.EndDrag += OnEndDrag;
    }

    private void UnRegister(Card card)
    {
        if (card.TryGetComponent(out CardMovement movement))
            _movements.Remove(movement);

        //card.BeginDrag -= OnBeginDrag;
        //card.EndDrag -= OnEndDrag;
    }
}
