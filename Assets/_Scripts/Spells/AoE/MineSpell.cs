using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpell : Spell
{
    private const string DispelleState = "Dispelled";
    private const string Growth = "Growth";
    private const string ExplosionSignal = "ExplosionSignal";

    [SerializeField] private FighterType _fighterType;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _vfx;

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
        Tick();
    }

    private IEnumerator Attack(UnitEnemy enemy, int damage, float delay)
    {
        _animator.Play(ExplosionSignal);

        yield return new WaitForSeconds(delay);

        int totalDamage = (int)DamageConditions.CalculateDamage(_fighterType, enemy.Fighter.FighterType, damage);
        enemy.Fighter.TakeDamage(totalDamage);

        _vfx.transform.SetParent(null);
        _vfx.Play();

        OnDispelled(this);

        Dispelled -= OnDispelled;
        WasCast -= OnCast;
    }

    private void OnCast(Cell cell, UnitStats stats)
    {
        _animator.Play(Growth);
        cell.StagedEnemyUnit += OnUnitStay;
    }

    private void OnUnitStay(UnitEnemy enemy)
    {
        StartCoroutine(Attack(enemy, CardSave.UnitStats.Damage, AffectDelay));

        Cell.StagedEnemyUnit -= OnUnitStay;
    }

    private void OnDispelled(Spell spell)
    {
        _animator.Play(DispelleState);
        Destroy(gameObject, 1f);
    }
}
