using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell : Spell
{
    [SerializeField] private ParticleSystem _vfx;
    [SerializeField] private FighterType _fighterType;

    private void OnEnable()
    {
        Dispelled += OnDispelled;
        WasCast += OnCast;
    }

    private void OnDisable()
    {
        Dispelled -= OnDispelled;
        WasCast -= OnCast;
    }

    protected override void Affect(Cell cell, UnitStats stats, float delay)
    {
        StartCoroutine(ApplyDamage(cell, stats.Damage, delay));
    }

    private IEnumerator ApplyDamage(Cell cell, int damage, float delay)
    {
        yield return new WaitForSeconds(delay);

        List<UnitEnemy> enemies = cell.GetEnemyUnits(DistanceAttacks);

        foreach (UnitEnemy enemy in enemies)
        {
            int totalDamage = (int)DamageConditions.CalculateDamage(_fighterType, enemy.Fighter.FighterType, damage);
            enemy.Fighter.TakeDamage(totalDamage);
        }
    }

    private void OnCast(Cell cell, UnitStats stats)
    {
        Affect(cell, stats, AffectDelay);
    }

    private void OnDispelled(Spell spell)
    {
        _vfx.Stop();
        Destroy(gameObject, 1f);
    }
}
