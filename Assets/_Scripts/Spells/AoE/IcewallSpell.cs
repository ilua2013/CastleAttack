using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcewallSpell : Spell
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
        StartCoroutine(Freeze(cell, delay));
    }

    private IEnumerator Freeze(Cell cell, float delay)
    {
        yield return new WaitForSeconds(delay);

        List<UnitEnemy> enemies = cell.GetEnemyUnits(DistanceAttacks);

        foreach (UnitEnemy enemy in enemies)
            enemy.SkipStep();
    }


    private void OnCast(Cell cell, UnitStats stats)
    {
        _cell = cell;
        _stats = stats;
    }

    private void OnFightStarted()
    {
        Debug.Log("Casty");
        Affect(_cell, _stats, AffectDelay);
    }

    private void OnDispelled(Spell spell)
    {
        gameObject.SetActive(false);
    }
}
