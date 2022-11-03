using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NewFighter
{
    [SerializeField] private FighterType _type;
    [SerializeField] private int _distanceAttack;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHealth;

    private int _health;

    public int Damage => _damage;
    public int Health => _health;
    public bool IsDead => _health < 1;
    public int MaxHealth => _maxHealth;
    public int DistanceAttack => _distanceAttack;
    public FighterType FighterType => _type;

    public event Action<NewFighter> Died_get;
    public event Action Died;
    public event Action<int> Damaged;
    public event Action<int> Healed;

    public void Init()
    {
        _health = _maxHealth;
    }

    public void Attack(NewFighter fighter)
    {
        int damage = (int)DamageConditions.CalculateDamage(_type, fighter.FighterType, _damage);
        fighter.TakeDamage(damage);

        if (fighter.FighterType == FighterType.MainTarget || fighter.FighterType == FighterType.MainWizzard) // получаем обратный урон если бьем по боссу
            TakeDamage(fighter.Damage);
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
}

public enum FighterType
{
    Catapult, Archer, Warrior, Tower, Build, MainTarget, MainWizzard
}
