using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    private const float Interval = 1f;

    [SerializeField] private float _lifetime;

    public event Action Dispelled;

    public void Cast(Action onEndCallback = null)
    {
        StartCoroutine(Live(onEndCallback));
    }

    protected abstract void Affect();

    private IEnumerator Live(Action onDeathCallback)
    {
        float elapsed = 0;
        float ticks = 0;

        while (elapsed < _lifetime)
        {
            if (ticks >= Interval)
            {
                ticks = 0;
                Affect();
            }

            ticks += Time.deltaTime;
            elapsed += Time.deltaTime;

            yield return null;
        }

        onDeathCallback?.Invoke();
        Dispelled?.Invoke();
    }
}
