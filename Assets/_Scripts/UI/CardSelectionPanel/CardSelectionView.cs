using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardSelectionView : MonoBehaviour
{
    private const float ScaleFactor = 1.2f;

    [SerializeField] private Transform[] _cardPlacements;

    private List<CardHoverView> _cardHovers = new List<CardHoverView>();
    private CardsSelection _cardsSelection;
    private DeckCounterView _deckCounterView;
    private bool _isActivTutor = false;

    private void Awake()
    {
        _cardsSelection = GetComponent<CardsSelection>();
        _deckCounterView = FindObjectOfType<DeckCounterView>();

        if (_cardsSelection == null)
            throw new NullReferenceException("This object must have a component " + nameof(CardsSelection));

        if (_deckCounterView == null)
            throw new NullReferenceException(nameof(_deckCounterView));
    }

    private void OnEnable()
    {
        _cardsSelection.DrawnOut += OnDrawOut;
    }

    private void OnDisable()
    {
        _cardsSelection.DrawnOut -= OnDrawOut;

        foreach (CardHoverView cardHover in _cardHovers)
            UnRegister(cardHover);
    }

    private void OnDrawOut(Card[] cards)
    {
        if (cards.Length != _cardPlacements.Length)
            throw new InvalidOperationException($"cards.Length != _cardPlacements.Length");

        for (int i = 0; i < _cardPlacements.Length; i++)
        {
            cards[i].transform.SetParent(_cardPlacements[i]);
            cards[i].transform.position = _deckCounterView.transform.position;

            if (cards[i].TryGetComponent(out CardHoverView cardHover))
            {
                cardHover.MoveTo(_cardPlacements[i].position, 5f, () => Register(cardHover));
                cardHover.ScaleTo(cardHover.StartScaling);

                _cardHovers.Add(cardHover);
            }
        }
    }

    private void OnCardHover(CardHoverView cardHover)
    {
        cardHover.SaveStartState(cardHover.transform.position, cardHover.transform.GetSiblingIndex());
        cardHover.ScaleTo(cardHover.StartScaling * ScaleFactor);
    }

    private void OnCardRemoveHover(CardHoverView cardHover)
    {
        cardHover.ResetToStartState();
    }

    private void Register(CardHoverView card)
    {
        card.SaveStartState(card.transform.position, card.transform.GetSiblingIndex());
        card.Enter += OnCardHover;
        card.Exit += OnCardRemoveHover;
    }

    private void UnRegister(CardHoverView card)
    {
        card.Enter -= OnCardHover;
        card.Exit -= OnCardRemoveHover;
    }
}
