using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcewallSpell : Spell
{
    private const string DispelleState = "Dispelle";
    private const string Growth = "Growth";

    [SerializeField] private FighterType _fighterType;
    [SerializeField] private Animator _animator;

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

    private void OnCast(Cell cell, UnitStats stats)
    {
        _animator.Play(Growth);
    }

    private void OnDispelled(Spell spell)
    {
        _animator.Play(DispelleState);
        Destroy(gameObject, 1f);
    }
}
