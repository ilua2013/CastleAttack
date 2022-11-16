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

    private void Start()
    {
        _cards = new List<Card>(_deck.Cards);

        foreach (Card card in _cards)
        {
            Register(card);

            if (card.TryGetComponent(out CardMovement movement))
            {
                movement.Init(_dragging);
                _movements.Add(movement);
            }
        }

        FillCards();
    }

    private void OnDisable()
    {
        foreach (Card card in _cards)
        {
            UnRegister(card);
        }
    }

    private void Update()
    {
        foreach (CardMovement movement in _movements)
            movement.Move();
    }

    public void AddCard(Card card)
    {
        Register(card);
        _cards.Add(card);
        _deck.Add(card);
        FillCards();
    }

    public void RemoveCard(Card card)
    {
        UnRegister(card);
        _cards.Remove(card);
        _deck.Remove(card);
        FillCards();
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
                    FillCards();
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

        FillCards();
    }

    private void FillCards()
    {
        for (int i = 0; i < _views.Count; i++)
        {
            for (int j = 0; j < _cards.Count; j++)
            {
                if (_cards[j].IsAvailable)
                {
                    _views[i].FillCard(_cards[j]);
                    break;
                }
            }
        }


        for (int i = 0; i < _cards.Count && i < _views.Count; i++)
        {
            if (_cards[i].IsAvailable == false)
            {
                continue;
            }
            
            _views[i].FillCard(_cards[i]);
        }
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        card.transform.SetParent(_dragging);
    }

    private void OnEndDrag(PointerEventData eventData, Card card)
    {
        TryTransferCard(eventData, card);
    }

    private void Register(Card card)
    {
        card.BeginDrag += OnBeginDrag;
        card.EndDrag += OnEndDrag;
    }

    private void UnRegister(Card card)
    {
        card.BeginDrag -= OnBeginDrag;
        card.EndDrag -= OnEndDrag;
    }
}
