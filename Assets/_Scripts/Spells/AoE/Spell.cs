using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [SerializeField] private DistanceAttack[] _distanceAttacks;
    [SerializeField] private float _affectDelay;
    [SerializeField] private float _stepTime;
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _maxTicks;
    [SerializeField] private bool _validWhenApplied;

    private BattleSystem _battleSystem;
    private int _ticks;
    private Cell _cell;
    private CardSave _save;

    public bool DoingStep { get; private set; }
    public bool ValidWhenApplied => _validWhenApplied;
    public DistanceAttack[] DistanceAttacks => _distanceAttacks;
    public float AffectDelay => _affectDelay;
    public float StepTime => _stepTime;
    public int MaxTicks => _maxTicks;
    public BattleSystem BattleSystem => _battleSystem;

    public Cell Cell => _cell;
    public CardSave CardSave => _save;

    public event Action<Spell> Dispelled;
    public event Action<Cell, UnitStats> WasCast;

    protected void Awake()
    {
        _ticks = _maxTicks;
    }

    public void DoStep()
    {
        DoingStep = true;
        Affect(_cell, _save.UnitStats, AffectDelay);
    }

    public void Cast(Cell cell, CardSave save, BattleSystem battleSystem, Action onEndCallback = null)
    {
        _cell = cell;
        _save = save;

        _battleSystem = battleSystem;
        _battleSystem.Win += OnWin;

        StartCoroutine(Live());
        WasCast?.Invoke(cell, save.UnitStats);
    }

    public void Tick()
    {
        _ticks--;
        DoingStep = false;
    }

    protected abstract void Affect(Cell cell, UnitStats stats, float delay);

    private void OnWin()
    {
        Dispelled?.Invoke(this);
    }

    private IEnumerator Live()
    {
        yield return new WaitUntil(() => _ticks <= 0);
        yield return new WaitForSeconds(_lifeTime);

        _battleSystem.Win -= OnWin;

        Dispelled?.Invoke(this);
    }
}
