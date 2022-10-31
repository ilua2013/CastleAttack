using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fighter : MonoBehaviour
{
    [SerializeField] private int _distanceAttack;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxHealth;

    private int _health;
    private MoverOnCell _mover;

    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
    public int Health => _health;

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
        List<Cell> cells = _mover.CurrentCell.GetForwardsCell(_distanceAttack);

        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].CurrentUnit != null && cells[i].CurrentUnit.TeamUnit != teamUnit)
            {                
                cells[i].CurrentUnit.Fighter.TakeDamage(_damage);
                return true;
            }
        }

        return false;
    }

    private void TakeDamage(int damage)
    {
        _health = Math.Clamp(_health - damage, 0, _maxHealth);

        Damaged?.Invoke(_health);

        if (_health == 0)
            Die();
    }

    private void Die()
    {
        Died?.Invoke(this);
    }
}
