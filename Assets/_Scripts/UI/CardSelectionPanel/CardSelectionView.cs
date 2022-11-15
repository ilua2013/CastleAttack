using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CardSelectionView : MonoBehaviour, IPhaseHandler
{
    [SerializeField] private Transform[] _cardPlacements;
    [SerializeField] private Phase[] _phases;

    private CardsSelection _cardsSelection;
    private float _delayTime = 0;
    private bool _isActivTutor = false;

    public Phase[] Phases => _phases;

    private void Awake()
    {
        _cardsSelection = GetComponent<CardsSelection>();


        if (_cardsSelection == null)
            throw new NullReferenceException("This object must have a component " + nameof(CardsSelection));
    }

    private void OnEnable()
    {
        _cardsSelection.DrawnOut += OnDrawOut;
    }

    private void OnDisable()
    {
        _cardsSelection.DrawnOut -= OnDrawOut;
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
    }

    private void OnDrawOut(Card[] cards)
    {
        if (cards.Length != _cardPlacements.Length)
            throw new InvalidOperationException($"cards.Length != _cardPlacements.Length");

        for (int i = 0; i < _cardPlacements.Length; i++)
        {
            cards[i].transform.SetParent(_cardPlacements[i]);
            cards[i].transform.localPosition = Vector3.zero;
        }
    }
}
