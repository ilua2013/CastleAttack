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
    [SerializeField] private BattleSystem _battleSystem;

    public Phase[] Phases => _phases;

    public event Action Opened;
    public event Action Closed;

    private void OnValidate()
    {
        _battleSystem = FindObjectOfType<BattleSystem>();
    }

    private void OnEnable()
    {
        _battleSystem.Win += OnOpen;   
    }

    private void OnDisable()
    {
        _battleSystem.Win -= OnOpen;
    }

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(phase.Delay);

        //if (phase.IsActive)
        //{
        //    StartCoroutine(ScalePanel(Vector3.zero, Vector3.one, true));
        //    Opened?.Invoke();
        //}
    }

    public void OpenPanel()
    {
        Debug.Log("open");
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
        float lerpTime = 2f;

        while (Vector3.Distance(_panel.localScale, to) > distanceDelta)
        {
            _panel.localScale = Vector3.Lerp(_panel.localScale, to, lerpTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        _panel.localScale = to;
    }
}
