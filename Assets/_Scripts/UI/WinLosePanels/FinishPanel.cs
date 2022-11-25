using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinishPanel : MonoBehaviour, IPhaseHandler
{
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _background;
    [SerializeField] private Phase[] _phases;

    public Phase[] Phases => _phases;

    public event Action Opened;

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(phase.Delay);

        if (phase.IsActive)
        {
            StartCoroutine(OpenPanel());
            Opened?.Invoke();
        }
    }

    private IEnumerator OpenPanel()
    {
        _panel.localScale = Vector3.zero;
        _background.gameObject.SetActive(true);

        float distanceDelta = 0.001f;
        float lerpTime = 2f;

        while (Vector3.Distance(_panel.localScale, Vector3.one) > distanceDelta)
        {
            _panel.localScale = Vector3.Lerp(_panel.localScale, Vector3.one, lerpTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _panel.localScale = Vector3.one;
    }
}
