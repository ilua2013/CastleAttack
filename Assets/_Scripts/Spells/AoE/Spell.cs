using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    private const float Interval = 1f;

    [SerializeField] private DistanceAttack[] _distanceAttacks;
    [SerializeField] private float _lifetime;

    public DistanceAttack[] DistanceAttacks => _distanceAttacks;

    public event Action Dispelled;
    public event Action WasCast;

    public void Cast(Cell cell, Action onEndCallback = null)
    {
        StartCoroutine(Live(onEndCallback, cell));
        WasCast?.Invoke();
    }

    protected abstract void Affect(Cell cell);

    private IEnumerator Live(Action onDeathCallback, Cell cell)
    {
        float elapsed = 0;
        float ticks = 0;

        while (elapsed < _lifetime)
        {
            if (ticks >= Interval)
            {
                ticks = 0;
                Affect(cell);
            }

            ticks += Time.deltaTime;
            elapsed += Time.deltaTime;

            yield return null;
        }

        onDeathCallback?.Invoke();
        Dispelled?.Invoke();
    }
}
