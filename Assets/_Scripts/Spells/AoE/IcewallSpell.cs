using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcewallSpell : Spell
{
    [SerializeField] private FighterType _fighterType;

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
        StartCoroutine(Freeze(cell, delay));
    }

    private IEnumerator Freeze(Cell cell, float delay)
    {
        yield return new WaitForSeconds(delay);

        List<UnitEnemy> enemies = cell.GetEnemyUnits(DistanceAttacks);

        foreach (UnitEnemy enemy in enemies)
            enemy.SkipStep();

        Tick();
    }

    private void OnDispelled(Spell spell)
    {
        gameObject.SetActive(false);
    }
}
