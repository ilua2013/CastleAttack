using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell : Spell
{
    [SerializeField] private int _damage;

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
            enemy.Fighter.TakeDamage(_damage);
    }

    private void OnDispelled()
    {
        gameObject.SetActive(false);
    }
}
