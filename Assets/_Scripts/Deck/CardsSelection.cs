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
    private Card[] _selectedCards;

    public event Action<Card[]> DrawnOut;
    public event Action<Card> CardSelected;
    
    public Phase[] Phases => _phases;

    private void Awake()
    {
        _deck = FindObjectOfType<CombatDeck>();
        _cardReplenisher = FindObjectOfType<CardReplenisher>();

        if (_deck == null)
            throw new NullReferenceException(nameof(_deck));

        if (_cardReplenisher == null)
            throw new NullReferenceException(nameof(_cardReplenisher));
    }

    public void SwitchPhase(PhaseType phaseType)
    {
        bool isActive = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType).IsActive;
        gameObject.SetActive(isActive);

        if (isActive)
            DrawOutCards();
    }

    private void DrawOutCards()
    {
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
    }
}
