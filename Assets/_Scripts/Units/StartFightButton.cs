using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StartFightButton : MonoBehaviour, IPhaseHandler
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _imageButton;
    [SerializeField] private Phase[] _phases;

    public Button Button => _button;
    public Phase[] Phases => _phases;

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(phase.Delay);

        _button.enabled = phase.IsActive;
        _imageButton.color = phase.IsActive ? Color.yellow : Color.gray;
    }
}
