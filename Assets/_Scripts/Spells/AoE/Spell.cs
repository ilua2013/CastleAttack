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
    private int _ticks;

    public DistanceAttack[] DistanceAttacks => _distanceAttacks;
    public float AffectDelay => _affectDelay;
    public int MaxTicks => _maxTicks;

    public event Action Dispelled;
    public event Action<Cell, UnitStats> WasCast;
    public event Action FightStarted;
    public event Action FightFinished;

    public void Cast(Cell cell, CardSave save, BattleSystem battleSystem, Action onEndCallback = null)
    {
        _battleSystem = battleSystem;

        //_battleSystem.StepStarted += OnStepStarted;
        _battleSystem.StepFinished += OnStepFinished;
        _battleSystem.Win += OnWin;

        StartCoroutine(Live());
        WasCast?.Invoke(cell, save.UnitStats);
    }

    public void Tick()
    {
        _ticks++;
    }

    protected abstract void Affect(Cell cell, UnitStats stats, float delay);

    private void OnStepStarted()
    {
        FightStarted?.Invoke();
    }

    private void OnStepFinished()
    {
        FightFinished?.Invoke();
    }

    private void OnWin()
    {
        Dispelled?.Invoke();
    }

    private IEnumerator Live()
    {
        yield return new WaitWhile(() => _ticks < _maxTicks);
        yield return new WaitForSeconds(_lifeTime);

        //_battleSystem.StepStarted -= OnStepStarted;
        _battleSystem.StepFinished -= OnStepFinished;
        _battleSystem.Win -= OnWin;

        Dispelled?.Invoke();
    }
}
