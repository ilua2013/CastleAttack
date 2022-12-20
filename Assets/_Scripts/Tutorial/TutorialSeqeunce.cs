using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialSeqeunce : MonoBehaviour, IPhaseHandler
{
    private const string HideState = "Hide";
    private const string ShowState = "Show";

    [SerializeField] private Animator _welcomePanel;
    [SerializeField] private Animator _selectionCardPanel;
    [SerializeField] private Animator _cardPlacementPanel;
    [SerializeField] private Animator _fightPanel;
    [SerializeField] private Animator _unitPanel;
    [SerializeField] private Animator _skipButtonPanel;
    [SerializeField] private Animator _wizzardPanel;
    [SerializeField] private Phase[] _phases;
    [SerializeField] private UnitSpawner[] _unitSpawners;

    private bool _selectionCard;
    private bool _cardPlacement;
    private bool _unit;
    private bool _skipButton;
    private bool _wizzard;

    public Phase[] Phases => _phases;

    private void OnValidate()
    {
        _unitSpawners = FindObjectsOfType<UnitSpawner>();
    }

    private void Start()
    {
        ShowHint(_welcomePanel);
    }

    private void OnEnable()
    {
        foreach (var item in _unitSpawners)
            item.SpawnedUnit += OnSpawnerUnit;
    }

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(phase.Delay);

        if (phase.IsActive)
            ShowHint(phase.PhaseType);
    }

    private void OnSpawnerUnit()
    {
        foreach (var item in _unitSpawners)
            item.SpawnedUnit -= OnSpawnerUnit;

        ShowHint(_fightPanel, 0.8f);
    }

    private void ShowHint(PhaseType phaseType)
    {
        if (phaseType == PhaseType.SelectionCard)
        {
            if (_selectionCard == false)
            {
                _selectionCard = true;
                StartCoroutine(Pausing(_selectionCardPanel));
            }
            else if (_unit == false)
            {
                _unit = true;
                ShowHint(_unitPanel);
            }
            else if (_wizzard == false)
            {
                _wizzard = true;
                ShowHint(_wizzardPanel);
            }
        }

        else if (phaseType == PhaseType.CardPlacement)
        {
            if (_cardPlacement == false)
            {
                _cardPlacement = true;
                StartCoroutine(Pausing(_cardPlacementPanel));
            }
            else if (_skipButton == false)
            {
                _skipButton = true;
                ShowHint(_skipButtonPanel);
            }
        }
    }

    private void ShowHint(Animator panel, float delay = 0)
    {
        StartCoroutine(Pausing(panel, delay));
    }

    private IEnumerator Pausing(Animator panel, float delay = 0)
    {
        yield return new WaitForSeconds(delay);

        panel.Play(ShowState);

        Time.timeScale = 0;

        while (!Input.anyKeyDown)
            yield return null;

        Time.timeScale = 1;

        panel.Play(HideState);
    }
}
