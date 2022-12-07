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

    private CombatDeck _deck;
    private CardsHand _cardsHand;
    private CardsHandView _cardsHandView;
    private DeckCounter _deckCounter;
    private Card[] _selectedCards;
    private float _delayTime = 0; 

    public event Action<Card[]> DrawnOut;
    public event Action CardSelected;
    public event Action Passed;

    public Phase[] Phases => _phases;

    private void Awake()
    {
        _deck = FindObjectOfType<CombatDeck>();
        _cardsHand = FindObjectOfType<CardsHand>();
        _deckCounter = FindObjectOfType<DeckCounter>();
        _cardsHandView = _cardsHand.GetComponent<CardsHandView>();

        if (_deck == null)
            throw new NullReferenceException(nameof(_deck));

        if (_cardsHand == null)
            throw new NullReferenceException(nameof(_cardsHand));

        if (_deckCounter == null)
            throw new NullReferenceException(nameof(_deckCounter));

        if (_cardsHandView == null)
            throw new NullReferenceException(nameof(_cardsHandView));
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

        yield return new WaitForSeconds(phase.Delay);       

        gameObject.SetActive(phase.IsActive);

        if (phase.IsActive)
            DrawOutCards();
    }

    private void DrawOutCards()
    {
        if (!_deckCounter.CanTakeCard)
        {
            Passed?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        if (_deck.IsEmpty)
        {
            CardSelected?.Invoke();
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
            _cardsHand.CardAdd(card, false);

        _cardsHandView.CardAdd(_selectedCards);

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
        CardSelected?.Invoke();
    }
}
