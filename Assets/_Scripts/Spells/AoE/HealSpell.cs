using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : Spell
{
    [SerializeField] private ParticleSystem _vfx;

    private Cell _cell;
    private UnitStats _stats;

    private void OnEnable()
    {
        Dispelled += OnDispelled;
        FightStarted += OnFightStarted;
        WasCast += OnCast;
    }

    private void OnDisable()
    {
        Dispelled -= OnDispelled;
        FightStarted -= OnFightStarted;
        WasCast -= OnCast;
    }

    protected override void Affect(Cell cell, UnitStats stats, float delay)
    {
        Tick();
        StartCoroutine(Heal(cell, stats.MaxHealth, delay));
    }

    private IEnumerator Heal(Cell cell, int value, float delay)
    {
        yield return new WaitForSeconds(delay);

        List<UnitFriend> units = cell.GetFriendUnits(DistanceAttacks);

        foreach (UnitFriend unit in units)
            unit.Fighter.RecoveryHealth(value);
    }

    private void OnCast(Cell cell, UnitStats stats)
    {
        _cell = cell;
        _stats = stats;
    }

    private void OnFightStarted()
    {
        Affect(_cell, _stats, AffectDelay);
    }

    private void OnDispelled()
    {
        _vfx.Stop();
        Destroy(gameObject, 1f);
    }
}
