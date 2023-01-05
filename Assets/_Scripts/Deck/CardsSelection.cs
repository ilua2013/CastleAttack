using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsSelection : MonoBehaviour, IPhaseHandler
{
    [SerializeField] private int _count;
    [SerializeField] private float _showDelay;
    [SerializeField] private Phase[] _phases;

    [SerializeField] private CombatDeck _deck;
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private CardsHandView _cardsHandView;
    [SerializeField] private DeckCounter _deckCounter;

    private Card[] _selectedCards;
    private float _delayTime = 0; 

    public event Action<Card[]> DrawnOut;
    public event Action<Card> CardReturned;
    public event Action CardSelected;
    public event Action Passed;

    public Phase[] Phases => _phases;

    private void OnValidate()
    {
        if (_deck == null)
            _deck = FindObjectOfType<CombatDeck>();

        if (_cardsHand == null)
            _cardsHand = FindObjectOfType<CardsHand>();

        if (_deckCounter == null)
            _deckCounter = FindObjectOfType<DeckCounter>();

        if (_cardsHandView == null)
            _cardsHandView = FindObjectOfType<CardsHandView>();
    }

    private void Awake()
    {
        _cardsHandView = _cardsHand.GetComponent<CardsHandView>();
    }

    public void TutorialPhaseEnable()
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == PhaseType.SelectionCard);
        phase.TutorialSelectionCardEnable();
    }

    public void TutorialPhaseDisable()
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == PhaseType.SelectionCard);
        phase.TutorialSelectionCardDisable();
    }

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        if (phase == null)
            yield break;

        yield return new WaitForSeconds(phase.Delay);       

        if (phase.IsActive)
            DrawOutCards();
    }

    private void DrawOutCards()
    {
        if (!_deckCounter.CanTakeCard)
        {
            Passed?.Invoke();
            return;
        }

        if (_deck.IsEmpty)
        {
            Passed?.Invoke();
            return;
        }

        _selectedCards = _deck.TakeRandomCards(_count);

        foreach (Card card in _selectedCards)
        {
            card.gameObject.SetActive(true);
            card.Activate(false);
        }

        DrawnOut?.Invoke(_selectedCards);

        StartCoroutine(OnCardSelect());
    }

    private IEnumerator OnCardSelect()
    {
        yield return new WaitForSeconds(_showDelay);

        foreach (Card card in _selectedCards)
        {
            if (_cardsHand.CanTakeCard)
            {
                _cardsHand.CardAdd(card, false);
                _cardsHandView.CardAdd(card);
            }
            else
            {
                _deck.ReturnCard(card);
                CardReturned?.Invoke(card);
            }
        }

        yield return new WaitForSeconds(0.5f);

        CardSelected?.Invoke();
    }
}
