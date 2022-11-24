using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinishPanel : MonoBehaviour, IPhaseHandler
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Phase[] _phases;

    public Phase[] Phases => _phases;

    public event Action Opened;

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(phase.Delay);

        if (phase.IsActive)
        {
            _panel.gameObject.SetActive(true);
            Opened?.Invoke();
        }
    }
}
