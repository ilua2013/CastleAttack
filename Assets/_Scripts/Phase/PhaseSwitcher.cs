using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSwitcher : MonoBehaviour
{
    private LevelSystem _levelSystem;
    private BattleSystem _battleSystem;
    private CardsSelection _cardSelection;

    private List<IPhaseHandler> _handlers = new List<IPhaseHandler>();

    public PhaseType CurrentPhase { get; private set; }

    private void Awake()
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
        _cardSelection.Passed += OnCardSelectionPassed;
        _levelSystem.Wave3Finished += OnFinished;
        _levelSystem.Failed += OnFailed;
    }

    private void OnDisable()
    {
        _battleSystem.StepStarted -= OnStepStarted;
        _battleSystem.StepFinished -= OnStepFinished;
        _cardSelection.CardSelected -= OnCardSelected;
        _cardSelection.Passed -= OnCardSelectionPassed;
        _levelSystem.Wave3Finished -= OnFinished;
        _levelSystem.Failed -= OnFailed;
    }

    private void Start()
    {
        Switch(PhaseType.SelectionCard);
        CurrentPhase = PhaseType.SelectionCard;
    }

    private void OnStepStarted()
    {
        Switch(PhaseType.Battle);
        CurrentPhase = PhaseType.Battle;
    }

    private void OnFailed()
    {
        Switch(PhaseType.Lose);
        CurrentPhase = PhaseType.Lose;
    }

    private void OnFinished()
    {
        Switch(PhaseType.Win);
        CurrentPhase = PhaseType.Win;
    }

    private void OnStepFinished()
    {
        Switch(PhaseType.SelectionCard);
        CurrentPhase = PhaseType.SelectionCard;
    }

    private void OnCardSelected(Card card)
    {
        Switch(PhaseType.CardPlacement);
        CurrentPhase = PhaseType.CardPlacement;
    }

    private void OnCardSelectionPassed()
    {
        Switch(PhaseType.CardPlacement);
        CurrentPhase = PhaseType.CardPlacement;
    }

    private void Switch(PhaseType phaseType)
    {
        if (CurrentPhase == PhaseType.Win || CurrentPhase == PhaseType.Lose)
            return;

        foreach (IPhaseHandler handler in _handlers)
            StartCoroutine(handler.SwitchPhase(phaseType));
    }
}
