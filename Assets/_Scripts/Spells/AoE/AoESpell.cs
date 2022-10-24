using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AoESpell : MonoBehaviour
{
    [SerializeField] private float _duration;

    public event Action Dispelled;

    public void Cast(Action onEndCallback = null)
    {
        StartCoroutine(Live(onEndCallback));
    }

    private void Dispel()
    {
        Dispelled?.Invoke();
    }

    private IEnumerator Live(Action onDeathCallback)
    {
        float elapsed = 0;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        onDeathCallback?.Invoke();
        Dispel();
    }
}
