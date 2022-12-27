using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallSpell : Spell
{
    private const string DispelleState = "Dispelle";

    [SerializeField] private FighterType _fighterType;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delayDamage = 0.2f;

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

            yield return new WaitForSeconds(_delayDamage);
        }

        Tick();
    }

    private void OnDispelled(Spell spell)
    {
        _animator.Play(DispelleState);
        Destroy(gameObject, 1f);
    }
}
