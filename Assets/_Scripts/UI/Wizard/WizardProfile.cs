using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WizardProfile : MonoBehaviour
{
    [SerializeField] private GameObject _background;
    [SerializeField] private Transform _panel;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;

    private Coroutine _coroutine;

    private void OnEnable()
    {
        _openButton.onClick.AddListener(Open);
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(Open);
        _closeButton.onClick.RemoveListener(Close);
    }

    private void Open()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _background.SetActive(true);
        _coroutine = StartCoroutine(Scale(Vector3.zero, Vector3.one, true));
    }

    private void Close()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _background.SetActive(false);
        _coroutine = StartCoroutine(Scale(Vector3.one, Vector3.zero, false));
    }

    private IEnumerator Scale(Vector3 from, Vector3 to, bool active)
    {
        _panel.localScale = from;

        if (active)
            _panel.gameObject.SetActive(active);

        float distanceDelta = 0.001f;
        float lerpTime = 8f;

        while (Vector3.Distance(_panel.localScale, to) > distanceDelta)
        {
            _panel.localScale = Vector3.MoveTowards(_panel.localScale, to, lerpTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        if (!active)
            _panel.gameObject.SetActive(active);

        _panel.localScale = to;
    }
}
