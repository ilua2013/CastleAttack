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
    [SerializeField] private bool _validWhenApplied;

    private BattleSystem _battleSystem;
    private int _ticks;

    public bool ValidWhenApplied => _validWhenApplied;
    public DistanceAttack[] DistanceAttacks => _distanceAttacks;
    public float AffectDelay => _affectDelay;
    public int MaxTicks => _maxTicks;
    public BattleSystem BattleSystem => _battleSystem;

    public event Action<Spell> Dispelled;
    public event Action<Cell, UnitStats> WasCast;
    public event Action FightStarted;
    public event Action FightFinished;

    protected void Awake()
    {
        _ticks = _maxTicks;
    }

    public void Cast(Cell cell, CardSave save, BattleSystem battleSystem, Action onEndCallback = null)
    {
        _battleSystem = battleSystem;

        _battleSystem.BattleStarted += OnStepStarted;
        _battleSystem.StepFinished += OnStepFinished;
        _battleSystem.Win += OnWin;

        StartCoroutine(Live());
        WasCast?.Invoke(cell, save.UnitStats);
    }

    public void Tick()
    {
        _ticks--;
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
        Dispelled?.Invoke(this);
    }

    private IEnumerator Live()
    {
        yield return new WaitUntil(() => _ticks <= 1);
        yield return new WaitForSeconds(_lifeTime);

        _battleSystem.BattleStarted -= OnStepStarted;
        _battleSystem.StepFinished -= OnStepFinished;
        _battleSystem.Win -= OnWin;

        Dispelled?.Invoke(this);
    }
}
