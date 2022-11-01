using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fighter : MonoBehaviour
{
    [SerializeField] private FighterType _type;
    [SerializeField] private int _distanceAttack;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHealth;

    private int _health;
    private MoverOnCell _mover;

    public int Damage => _damage;
    public int Health => _health;
    public bool IsDead => _health < 1;
    public int MaxHealth => _maxHealth;
    public FighterType FighterType => _type;

    public event Action<Fighter> Died;
    public event Action<int> Damaged;
    public event Action<int> Healed;

    private void Awake()
    {
        _mover = GetComponent<MoverOnCell>();
        _health = _maxHealth;
    }

    public bool TryAttack(TeamUnit teamUnit)
    {
        List<Cell> cells;

        if (teamUnit == TeamUnit.Friend)
            cells = _mover.CurrentCell.GetForwardsCell(_distanceAttack);
        else
            cells = _mover.CurrentCell.GetBottomCell(_distanceAttack);

        for (int i = 0; i < cells.Count; i++)
        {
            UnitStep unit = cells[i].CurrentUnit;

            if (unit != null && unit.TeamUnit != teamUnit && unit.Fighter.IsDead == false)
            {
                int damage = (int)DamageConditions.CalculateDamage(_type, unit.Fighter.FighterType, _damage);
                unit.Fighter.TakeDamage(damage);

                return true;
            }
        }

        return false;
    }

    private void TakeDamage(int damage)
    {
        _health = Math.Clamp(_health - damage, 0, _maxHealth);

        Damaged?.Invoke(damage);

        if (_health == 0)
            Die();
    }

    private void Die()
    {
        Died?.Invoke(this);
        gameObject.SetActive(false);
    }
}

public enum FighterType
{
    Catapult, Archer, Warrior, Tower
}
