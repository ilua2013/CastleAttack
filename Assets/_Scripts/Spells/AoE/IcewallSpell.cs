using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcewallSpell : Spell
{
    [SerializeField] private FighterType _fighterType;

    private List<UnitEnemy> _enemies;

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
        StartCoroutine(Freeze(cell, delay));
    }

    private IEnumerator Freeze(Cell cell, float delay)
    {
        yield return new WaitForSeconds(delay);

        _enemies = cell.GetEnemyUnits(DistanceAttacks);

        foreach (UnitEnemy enemy in _enemies)
            enemy.Mover.SetMove(false);
    }

    private void OnDispelled()
    {
        foreach (UnitEnemy enemy in _enemies)
            enemy.Mover.SetMove(true);

        gameObject.SetActive(false);
    }
}
