using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZippSpell : Spell
{
    [SerializeField] private ZippProjectile _projectile;
    [SerializeField] private int _bounces;
    [SerializeField] private FighterType _fighterType;

    private Transform _startPoint;
    private int _damage;

    private void Awake()
    {
        _startPoint = FindObjectOfType<StaffPoint>().transform;
    }

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
    }

    private void ApplyDamage(UnitEnemy enemy, int damage)
    {
        int totalDamage = (int)DamageConditions.CalculateDamage(_fighterType, enemy.Fighter.FighterType, damage);
        enemy.Fighter.TakeDamage(totalDamage);
    }

    private void OnCast(Cell cell, UnitStats stats)
    {
        _damage = stats.Damage;

        List<UnitEnemy> enemies = new List<UnitEnemy>();

        foreach (UnitEnemy enemy in BattleSystem.UnitsEnemy)
        {
            if (enemies.Count >= _bounces)
                break;

            enemies.Add(enemy);
        }

        if (enemies.Count <= 0)
            return;

        ZippProjectile projectile = Instantiate(_projectile, _startPoint.position, Quaternion.identity);
        StartCoroutine(FlyProjectile(enemies, projectile));
    }

    private IEnumerator FlyProjectile(List<UnitEnemy> targets, ZippProjectile projectile)
    {
        int currentBounce = 0;

        for (int i = 0; i < targets.Count; i++)
        {
            projectile.ResetState();
            projectile.FlyTo(targets[i].transform.position + Vector3.up);

            yield return new WaitUntil(() => projectile.IsTargetReached);

            ApplyDamage(targets[i], _damage);

            if (currentBounce >= _bounces - 1)
                break;

            if (i >= targets.Count - 1 && i < _bounces && i > 0)
                i -= 2;

            currentBounce++;
        }

        Destroy(projectile.gameObject, 0.3f);
    }

    private void OnDispelled()
    {
        Destroy(gameObject, 1f);
    }
}
