using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FinishPanel))]
public class SoundEffectPanel : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer _settings;

    private FinishPanel _finishPanel;

    private void Awake()
    {
        _finishPanel = GetComponent<FinishPanel>();
    }

    private void OnEnable()
    {
        _finishPanel.Opened += OnOpened;
    }

    private void OnDisable()
    {
        _finishPanel.Opened -= OnOpened;
    }

    private void OnOpened()
    {
        StartCoroutine(PlayWithDelay(0.2f, () =>
        _settings.Play(SoundEffectType.Panel)));
    }

    private IEnumerator PlayWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();
    }
}
