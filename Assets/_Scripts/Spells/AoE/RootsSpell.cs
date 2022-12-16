using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsSpell : Spell
{
    private List<UnitEnemy> _enemies;

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
        StartCoroutine(Root(delay));
    }

    private void OnCast(Cell cell, UnitStats stats)
    {
        _enemies = BattleSystem.UnitsEnemy;

        foreach (UnitEnemy enemy in _enemies)
            enemy.Mover.Root();
    }

    private IEnumerator Root(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (UnitEnemy enemy in _enemies)
            enemy.Mover.SkipStep();

        Tick();
    }

    private void OnDispelled(Spell spell)
    {
        foreach (UnitEnemy enemy in _enemies)
            enemy.Mover.UnRoot();

        Destroy(gameObject, 1f);
    }
}
