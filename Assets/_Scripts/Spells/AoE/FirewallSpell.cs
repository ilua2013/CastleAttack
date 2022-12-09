using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallSpell : Spell
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

    protected override void Affect(Cell cell, CardSave save, float delay)
    {
        StartCoroutine(Fire(cell, _damage, delay));
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

    private void OnDispelled()
    {
        gameObject.SetActive(false);
    }
}
