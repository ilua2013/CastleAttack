using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallSpell : Spell
{
    [SerializeField] private FighterType _fighterType;

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
        StartCoroutine(Fire(cell, stats.Damage, delay));
    }

    private IEnumerator Fire(Cell cell, int damage, float delay)
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
        _cell = cell;
        _stats = stats;
    }

    private void OnFightStarted()
    {
        Affect(_cell, _stats, AffectDelay);
    }

    private void OnDispelled(Spell spell)
    {
        gameObject.SetActive(false);
    }
}
