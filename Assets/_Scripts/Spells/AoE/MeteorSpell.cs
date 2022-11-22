using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell : Spell
{
    [SerializeField] private int _damage;
    [SerializeField] private FighterType _fighterType;

    public int Damage => _damage;

    private void OnEnable()
    {
        Dispelled += OnDispelled;
    }

    private void OnDisable()
    {
        Dispelled -= OnDispelled;
    }

    protected override void Affect(Cell cell)
    {
        List<UnitEnemy> enemies = cell.GetEnemyUnits(DistanceAttacks);

        foreach (UnitEnemy enemy in enemies)
        {
            int damage = (int)DamageConditions.CalculateDamage(_fighterType, enemy.Fighter.FighterType, _damage);
            enemy.Fighter.TakeDamage(damage);
        }
    }

    private void OnDispelled()
    {
        gameObject.SetActive(false);
    }
}
