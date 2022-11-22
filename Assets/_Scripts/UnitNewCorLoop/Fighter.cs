using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Fighter
{
    [SerializeField] private FighterType _type;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _delayAttack;
    [SerializeField] private float _timeToDefaultRotate;
    [SerializeField] private float _speedRotate;

    private IUnit _unit;
    private Transform transform;
    private int _health;

    public int Damage => _damage;
    public int Health => _health;
    public bool IsDead => _health < 1;
    public int MaxHealth => _maxHealth;
    public FighterType FighterType => _type;
    public IUnit Unit => _unit;

    public event Action Died;
    public event Action Attacked;
    public event Action<int> Damaged;
    public event Action<int> Healed;
    public event Action<Fighter> Died_get;

    public void Init(IUnit unit, Transform transformUnit)
    {
        transform = transformUnit;
        _unit = unit;
        _health = _maxHealth;
    }

    public void Attack(Fighter fighter)
    {
        _unit.RotateTo(fighter.transform);

        int damage = (int)DamageConditions.CalculateDamage(_type, fighter.FighterType, _damage);
        fighter.TakeDamage(damage);

        if (fighter.FighterType == FighterType.MainTarget || fighter.FighterType == FighterType.MainWizzard) // получаем обратный урон если бьем по боссу
            TakeDamage(fighter.Damage);

        Attacked?.Invoke();
    }

    public void RecoveryHealth(int value)
    {
        if (IsDead)
            return;

        _health += value;
        Healed?.Invoke(_health);
    }

    public void Die()
    {
        Died_get?.Invoke(this);
        Died?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        _health = Math.Clamp(_health - damage, 0, _maxHealth);

        Damaged?.Invoke(damage);

        if (_health == 0)
            Die();
    }

    public IEnumerator RotateTo(Transform lookAt)
    {
        Vector3 startRotate = transform.forward;
        Vector3 target = lookAt.position - transform.position;
        target.y = 0;

        while (Vector3.Distance(transform.forward, target.normalized) > 0.1f)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, target, _speedRotate * Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(_timeToDefaultRotate);

        while (transform.forward != startRotate)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, startRotate, _speedRotate * Time.deltaTime);

            yield return null;
        }
    }
}

public enum FighterType
{
    Catapult, Archer, Warrior, Tower, Build, MainTarget, MainWizzard, AttackSpell
}
