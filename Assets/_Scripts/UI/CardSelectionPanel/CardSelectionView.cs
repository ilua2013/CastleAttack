using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardSelectionView : MonoBehaviour
{
    private const float ScaleFactor = 1.32f;

    [SerializeField] private Transform[] _cardPlacements;

    private List<CardHoverView> _cardHovers = new List<CardHoverView>();
    private CardsSelection _cardsSelection;
    private DeckCounterView _deckCounterView;
    private bool _isActivTutor = false;

    private void Awake()
    {
        _cardsSelection = GetComponent<CardsSelection>();
        _deckCounterView = FindObjectOfType<DeckCounterView>(true);

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
                Register(cardHover, _cardPlacements[i].position);
                cardHover.MoveTo(_cardPlacements[i].position, 5f);
                cardHover.ScaleTo(cardHover.StartScaling * ScaleFactor);

                _cardHovers.Add(cardHover);
            }
        }
    }

    private void OnCardHover(CardHoverView cardHover)
    {
        cardHover.ScaleTo(cardHover.StartScaling * ScaleFactor * 1.2f);
    }

    private void OnCardRemoveHover(CardHoverView cardHover)
    {
        cardHover.ScaleTo(cardHover.StartScaling * ScaleFactor);
    }

    private void Register(CardHoverView card, Vector3 startPosition)
    {
        card.SaveStartState(startPosition, card.transform.GetSiblingIndex());
        card.Enter += OnCardHover;
        card.Exit += OnCardRemoveHover;
    }

    private void UnRegister(CardHoverView card)
    {
        card.Enter -= OnCardHover;
        card.Exit -= OnCardRemoveHover;
    }
}
