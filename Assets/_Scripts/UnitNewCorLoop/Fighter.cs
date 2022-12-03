using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Fighter
{
    public Transform transform { get; private set; }

    [SerializeField] private FighterType _type;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHealth;

    private IUnit _unit;
    private Vector3 _startRotate;
    private float _timeToDefaultRotate = 0.5f;
    private float _speedRotate = 2.5f;
    private int _health;

    public int Damage => _damage;
    public int Health => _health;
    public bool IsDead => _health < 1;
    public int MaxHealth => _maxHealth;
    public float RemainingHealth => (float)Health / MaxHealth;
    public FighterType FighterType => _type;
    public IUnit Unit => _unit;

    public event Action Died;
    public event Action ReadyToDie;
    public event Action EffectDied;
    public event Action<Transform> Died_getKiller;
    public event Action Attacked;
    public event Action<int> Damaged;
    public event Action<int> Healed;
    public event Action<Fighter> Died_get;

    public void Init(IUnit unit, Transform transformUnit)
    {
        transform = transformUnit;
        _unit = unit;
        _health = _maxHealth;
        _startRotate = transform.forward;
    }

    public void Init(IUnit unit, Transform transformUnit, int damage, int health)
    {
        transform = transformUnit;
        _unit = unit;

        _maxHealth = health;
        _health = _maxHealth;
        _damage = damage;

        _startRotate = transform.forward;
    }

    public bool Attack(Fighter fighter)
    {
        _unit.RotateTo(fighter.transform);

        int damage = (int)DamageConditions.CalculateDamage(_type, fighter.FighterType, _damage);
        bool isFatal = fighter.TakeDamage(this);

        if (fighter.FighterType == FighterType.MainTarget || fighter.FighterType == FighterType.MainWizzard) // получаем обратный урон если бьем по боссу
            TakeDamage(fighter);

        Attacked?.Invoke();

        return isFatal;
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

    public bool TakeDamage(int damage)
    {
        if (FighterType == FighterType.MainWizzard)
            if (_health <= damage)
            {
                ReadyToDie?.Invoke();
                return true;
            }

        _health = Math.Clamp(_health - damage, 0, _maxHealth);

        Damaged?.Invoke(damage);

        if (_health <= 0)
        {
            Die();
            EffectDied?.Invoke();

            return true;
        }

        return false;
    }

    public bool TakeDamage(Fighter fighter)
    {
        if (TakeDamage(fighter.Damage))
        {
            Died_getKiller?.Invoke(fighter.transform);
            return true;
        }

        return false;
    }

    public IEnumerator RotateTo(Transform lookAt, Action onFinish = null)
    {
        Vector3 target = lookAt.position - transform.position;
        target.y = 0;

        while (Vector3.Distance(transform.forward, target.normalized) > 0.1f)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, target, _speedRotate * Time.deltaTime);

            yield return null;
        }

        yield return new WaitForSeconds(_timeToDefaultRotate);

        while (transform.forward != _startRotate)
        {
            transform.forward = Vector3.MoveTowards(transform.forward, _startRotate, _speedRotate * Time.deltaTime);

            yield return null;
        }

        onFinish?.Invoke();
    }
}

public enum FighterType
{
    Catapult, Archer, Warrior, Tower, Build, MainTarget, MainWizzard, AttackSpell
}
