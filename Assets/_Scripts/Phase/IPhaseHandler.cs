using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhaseHandler
{
    public Phase[] Phases { get; }
    public IEnumerator SwitchPhase(PhaseType phaseType);
}
