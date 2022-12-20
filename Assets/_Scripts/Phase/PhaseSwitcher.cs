using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSwitcher : MonoBehaviour
{
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private CardsSelection _cardSelection;

    private List<IPhaseHandler> _handlers = new List<IPhaseHandler>();

    public PhaseType CurrentPhase { get; private set; }

    public event Action<PhaseType> PhaseSwitched;

    private void OnValidate()
    {
        if (_battleSystem == null)
            _battleSystem = FindObjectOfType<BattleSystem>(true);

        if (_cardSelection == null)
            _cardSelection = FindObjectOfType<CardsSelection>(true);
    }

    private void Awake()
    {
        foreach (var item in FindObjectsOfType<MonoBehaviour>(true))
        {
            if (item is IPhaseHandler handler)
                _handlers.Add(handler);
        }
    }

    private void Start()
    {
        Switch(PhaseType.SelectionCard);
        CurrentPhase = PhaseType.SelectionCard;
    }

    private void OnEnable()
    {
        _battleSystem.Win += OnFinished;
        _battleSystem.Lose += OnFailed;
        _battleSystem.Revived += OnRevived;
        _battleSystem.BattleStarted += OnStepStarted;
        _battleSystem.StepFinished += OnStepFinished;
        _cardSelection.CardSelected += OnCardSelected;
        _cardSelection.Passed += OnCardSelectionPassed;
    }

    private void OnDisable()
    {
        _battleSystem.Win -= OnFinished;
        _battleSystem.Lose -= OnFailed;
        _battleSystem.Revived -= OnRevived;
        _battleSystem.BattleStarted -= OnStepStarted;
        _battleSystem.StepFinished -= OnStepFinished;
        _cardSelection.CardSelected -= OnCardSelected;
        _cardSelection.Passed -= OnCardSelectionPassed;
    }

    public void Register(IPhaseHandler phaseHandler)
    {
        _handlers.Add(phaseHandler);
    }

    public void UnRegister(IPhaseHandler phaseHandler)
    {
        _handlers.Remove(phaseHandler);
    }

    private void OnStepStarted()
    {
        Switch(PhaseType.Battle);
        CurrentPhase = PhaseType.Battle;
    }

    private void OnRevived()
    {
        CurrentPhase = PhaseType.Battle;
        Switch(PhaseType.Battle);
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
        if (_battleSystem.CountEnemy <= 0)
            return;

        Switch(PhaseType.SelectionCard);
        CurrentPhase = PhaseType.SelectionCard;
    }

    private void OnCardSelected()
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

        PhaseSwitched?.Invoke(phaseType);
    }
}
