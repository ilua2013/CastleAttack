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

    public int Damage => _damage;
    public int Health => _health;
    public bool IsDead => _health < 1;
    public int MaxHealth => _maxHealth;

    public event Action<Fighter> Died;
    public event Action<int> Damaged;
    public event Action<int> Healed;

    private void Awake()
    {
        _mover = GetComponent<MoverOnCell>();
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
            print("Cells count - " + cells.Count + " " + cells[i].gameObject.name);
            UnitStep unit = cells[i].CurrentUnit;

            if (unit != null && unit.TeamUnit != teamUnit && unit.Fighter.IsDead == false)
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
        print("TakeDamage " + gameObject.name);
        Damaged?.Invoke(_health);

        if (_health == 0)
            Die();
    }

    private void Die()
    {
        Died?.Invoke(this);
        gameObject.SetActive(false);
    }
}
