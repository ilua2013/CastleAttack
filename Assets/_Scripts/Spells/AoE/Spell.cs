using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [SerializeField] private DistanceAttack[] _distanceAttacks;
    [SerializeField] private float _affectDelay;
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _maxTicks;

    private BattleSystem _battleSystem;
    private Cell _cell;
    private CardSave _save;
    private int _ticks;

    public DistanceAttack[] DistanceAttacks => _distanceAttacks;

    public event Action Dispelled;
    public event Action WasCast;

    public void Cast(Cell cell, CardSave save, BattleSystem battleSystem, Action onEndCallback = null)
    {
        _battleSystem = battleSystem;
        _cell = cell;
        _save = save;

        StartCoroutine(Live());

        _battleSystem.StepFinished += OnStepFinished;
        OnStepFinished();

        WasCast?.Invoke();
    }

    protected abstract void Affect(Cell cell, CardSave save, float delay);

    private void OnStepFinished()
    {
        Affect(_cell, _save, _affectDelay);
        _ticks++;
    }

    private IEnumerator Live()
    {
        yield return new WaitWhile(() => _ticks < _maxTicks);
        yield return new WaitForSeconds(_lifeTime);

        _battleSystem.StepFinished -= OnStepFinished;
        Dispelled?.Invoke();
    }
}
