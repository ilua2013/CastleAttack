using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinishPanel : MonoBehaviour, IPhaseHandler
{
    public bool Win;
    [SerializeField] private Transform _panel;
    [SerializeField] private Transform _background;
    [SerializeField] private Phase[] _phases;

    public Phase[] Phases => _phases;

    public event Action Opened;
    public event Action Closed;

    private void Update()
    {
        if (Win)
        {
            Win = false;
            OnOpen();
        }
    }

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        if (phase == null)
            yield break;

        yield return new WaitForSeconds(phase.Delay);

        if (phase.IsActive)
            OnOpen();
    }

    public void OpenPanel()
    {
        StartCoroutine(ScalePanel(Vector3.zero, Vector3.one, true));
    }

    public void ClosePanel()
    {
        StartCoroutine(ScalePanel(Vector3.one, Vector3.zero, false));
    }

    private void OnOpen()
    {
        Opened?.Invoke();
    }

    private void OnClose()
    {
        Closed?.Invoke();
    }

    private IEnumerator ScalePanel(Vector3 from, Vector3 to, bool isOpening)
    {
        _panel.localScale = from;
        _background.gameObject.SetActive(isOpening);

        float distanceDelta = 0.001f;
        float lerpTime = 3.5f;

        while (Vector3.Distance(_panel.localScale, to) > distanceDelta)
        {
            _panel.localScale = Vector3.Lerp(_panel.localScale, to, lerpTime * Time.deltaTime);
            yield return null;
        }

        _panel.localScale = to;
    }
}
