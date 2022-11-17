using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsSelection : MonoBehaviour, IPhaseHandler
{
    [SerializeField] private int _count;
    [SerializeField] private Phase[] _phases;

    private CombatDeck _deck;
    private CardReplenisher _cardReplenisher;
    private CardsHand _cardsHand;
    private Card[] _selectedCards;
    private float _delayTime = 0; 

    public event Action<Card[]> DrawnOut;
    public event Action<Card> CardSelected;
    public event Action Passed;

    public Phase[] Phases => _phases;

    private void Awake()
    {
        _deck = FindObjectOfType<CombatDeck>();
        _cardReplenisher = FindObjectOfType<CardReplenisher>();
        _cardsHand = FindObjectOfType<CardsHand>();

        if (_deck == null)
            throw new NullReferenceException(nameof(_deck));

        if (_cardReplenisher == null)
            throw new NullReferenceException(nameof(_cardReplenisher));

        if (_cardsHand == null)
            throw new NullReferenceException(nameof(_cardsHand));
    }

    public void TutorialTimeSwitch(float time)
    {
        _delayTime = time;
    }

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(_delayTime);

        _delayTime = phase.Delay;

        gameObject.SetActive(phase.IsActive);

        if (phase.IsActive)
            DrawOutCards();
    }

    private void DrawOutCards()
    {
        if (!_cardsHand.CanTakeCard)
        {
            Passed?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        _selectedCards = _deck.ShowRandomCards(_count);

        foreach (Card card in _selectedCards)
        {
            card.gameObject.SetActive(true);
            card.Clicked += OnCardSelect;
        }

        DrawnOut?.Invoke(_selectedCards);
    }

    private void OnCardSelect(PointerEventData eventData, Card card)
    {
        foreach (Card selectedCard in _selectedCards)
        {
            selectedCard.Clicked -= OnCardSelect;
            selectedCard.gameObject.SetActive(false);
        }

        _deck.ReturnCards(_selectedCards);
        _cardReplenisher.Create(card, eventData.position);

        CardSelected?.Invoke(card);
        gameObject.SetActive(false);
    }
}
