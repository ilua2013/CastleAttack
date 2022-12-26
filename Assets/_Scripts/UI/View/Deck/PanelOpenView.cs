using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpenView : MonoBehaviour
{
    [SerializeField] private float _scale;

    private Coroutine _coroutineScale;
    private bool _isOpened;

    public void Open()
    {
        if (_isOpened)
            return;

        _isOpened = true;

        if (_coroutineScale != null)
            StopCoroutine(_coroutineScale);

        gameObject.SetActive(true);
        _coroutineScale = StartCoroutine(Scale(Vector3.zero, Vector3.one * _scale));
    }

    public void Close()
    {
        if (_isOpened == false)
            return;

        _isOpened = false;

        if (_coroutineScale != null)
            StopCoroutine(_coroutineScale);

        if (gameObject.activeInHierarchy == false)
            return;

        _coroutineScale = StartCoroutine(Scale(Vector3.one * _scale, Vector3.zero, () => gameObject.SetActive(false)));
    }

    private IEnumerator Scale(Vector3 from, Vector3 to, Action onEnd = null)
    {
        transform.localScale = from;

        float distanceDelta = 0.001f;
        float lerpTime = 8f;

        while (Vector3.Distance(transform.localScale, to) > distanceDelta)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, to, lerpTime * Time.deltaTime);
            yield return null;
        }

        transform.localScale = to;
        onEnd?.Invoke();
    }
}
