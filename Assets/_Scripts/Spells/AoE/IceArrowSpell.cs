using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceArrowSpell : Spell
{
    private const string DispelleState = "Dispelle";
    private const string Growth = "Growth";

    [SerializeField] private ParticleSystem _vfx;
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

    private IEnumerator ApplyDamage(Cell cell, int damage, float delay)
    {
        _vfx.Play();

        yield return new WaitForSeconds(delay);

        List<UnitEnemy> enemies = cell.GetEnemyUnits(DistanceAttacks);

        foreach (UnitEnemy enemy in enemies)
        {
            int totalDamage = (int)DamageConditions.CalculateDamage(_fighterType, enemy.Fighter.FighterType, damage);
            enemy.Fighter.TakeDamage(totalDamage);
        }

        _animator.Play(Growth);
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
        StartCoroutine(ApplyDamage(cell, stats.Damage, AffectDelay));
    }

    private void OnDispelled(Spell spell)
    {
        _animator.Play(DispelleState);
        Destroy(gameObject, 1f);
    }
}
