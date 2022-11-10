using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSwitcher : MonoBehaviour
{
    private LevelSystem _levelSystem;
    private BattleSystem _battleSystem;
    private CardsSelection _cardSelection;

    private List<IPhaseHandler> _handlers = new List<IPhaseHandler>();

    private void OnValidate()
    {
        _levelSystem = FindObjectOfType<LevelSystem>(true);
        _battleSystem = FindObjectOfType<BattleSystem>(true);
        _cardSelection = FindObjectOfType<CardsSelection>(true);

        foreach (var item in FindObjectsOfType<MonoBehaviour>(true))
        {
            if (item is IPhaseHandler handler)
                _handlers.Add(handler);
        }
    }

    private void OnEnable()
    {
        _battleSystem.StepStarted += OnStepStarted;
        _battleSystem.StepFinished += OnStepFinished;
        _cardSelection.CardSelected += OnCardSelected;
    }

    private void OnDisable()
    {
        _battleSystem.StepStarted -= OnStepStarted;
        _battleSystem.StepFinished -= OnStepFinished;
        _cardSelection.CardSelected -= OnCardSelected;
    }

    private void Start()
    {
        Switch(PhaseType.SelectionCard);
    }

    private void OnStepStarted()
    {
        Switch(PhaseType.Battle);
    }

    private void OnStepFinished()
    {
        Switch(PhaseType.SelectionCard);
    }

    private void OnCardSelected(Card card)
    {
        Debug.Log("Card Selecrt");
        Switch(PhaseType.CardPlacement);
    }

    private void Switch(PhaseType phaseType)
    {
        Debug.Log("Phase switch to " + phaseType);
        foreach (IPhaseHandler handler in _handlers)
        {
            handler.SwitchPhase(phaseType);
        }
    }
}
