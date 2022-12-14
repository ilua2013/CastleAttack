using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : Spell
{
    [SerializeField] private ParticleSystem _vfx;

    private void OnEnable()
    {
        Dispelled += OnDispelled;
    }

    private void OnDisable()
    {
        Dispelled -= OnDispelled;
    }

    protected override void Affect(Cell cell, UnitStats stats, float delay)
    {
        StartCoroutine(Heal(cell, stats.MaxHealth, delay));
    }

    private IEnumerator Heal(Cell cell, int value, float delay)
    {
        yield return new WaitForSeconds(delay);

        List<UnitFriend> units = cell.GetFriendUnits(DistanceAttacks);

        foreach (UnitFriend unit in units)
            unit.Fighter.RecoveryHealth(value);

        Tick();
    }

    private void OnDispelled(Spell spell)
    {
        _vfx.Stop();
        Destroy(gameObject, 1f);
    }
}
