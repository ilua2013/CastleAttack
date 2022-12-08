using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardSelectionView : MonoBehaviour
{
    private const float ScaleFactor = 0.2f;
    private const float MoveSpeed = 8f;

    [SerializeField] private Transform[] _cardPlacements;

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
        _cardsSelection.CardReturned += OnReturned;
    }

    private void OnDisable()
    {
        _cardsSelection.DrawnOut -= OnDrawOut;
        _cardsSelection.CardReturned -= OnReturned;
    }

    private void OnDrawOut(Card[] cards)
    {
        for (int i = 0; i < _cardPlacements.Length; i++)
        {
            if (i >= cards.Length)
            {
                _cardPlacements[i].gameObject.SetActive(false);
                continue;
            }

            cards[i].transform.SetParent(_cardPlacements[i]);
            cards[i].transform.position = _deckCounterView.transform.position;
            cards[i].transform.localScale = cards[i].transform.localScale * ScaleFactor;

            if (cards[i].TryGetComponent(out CardHoverView cardHover))
                MoveCard(cardHover, _cardPlacements[i].position, cardHover.StartScaling);
        }
    }

    private void OnReturned(Card card)
    {
        if (card.TryGetComponent(out CardHoverView cardHover))
            MoveCard(cardHover, _deckCounterView.transform.position, cardHover.transform.localScale * ScaleFactor, () => cardHover.gameObject.SetActive(false));
    }

    private void MoveCard(CardHoverView cardHover, Vector3 position, Vector3 startScale, Action onReached = null)
    {
        cardHover.SaveStartState(position, cardHover.transform.GetSiblingIndex());
        cardHover.MoveTo(position, MoveSpeed, onReached);
        cardHover.ScaleTo(startScale);
    }
}
