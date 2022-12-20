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
    [SerializeField] private Phase[] _phases;

    public Phase[] Phases => _phases;

    private void Start()
    {
        ShowHint(_welcomePanel);
    }

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(phase.Delay);

        if (phase.IsActive)
            ShowHint(phase.PhaseType);
    }

    private void ShowHint(PhaseType phaseType)
    {
        if (phaseType == PhaseType.SelectionCard)
            StartCoroutine(Pausing(_selectionCardPanel));

        else if (phaseType == PhaseType.CardPlacement)
            StartCoroutine(Pausing(_cardPlacementPanel));
    }

    private void ShowHint(Animator panel)
    {
        StartCoroutine(Pausing(panel));
    }

    private IEnumerator Pausing(Animator panel)
    {
        panel.Play(ShowState);

        Time.timeScale = 0;

        while (!Input.anyKeyDown)
            yield return null;

        Time.timeScale = 1;

        panel.Play(HideState);
    }
}
