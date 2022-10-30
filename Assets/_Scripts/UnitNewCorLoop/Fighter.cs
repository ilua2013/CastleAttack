using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fighter : MonoBehaviour
{
    [SerializeField] private int _distanceAttack;
    [SerializeField] private int _damage;
    [SerializeField] private int _health;

    private MoverOnCell _mover;

    public int Damage => _damage;

    public event Action<Fighter> Died;

    private void Awake()
    {
        _mover = GetComponent<MoverOnCell>();
    }

    public bool TryAttack(TeamUnit teamUnit)
    {
        List<Cell> cells = _mover.CurrentCell.GetForwardsCell(_distanceAttack);

        for (int i = 0; i < cells.Count; i++)
        {
<<<<<<< Updated upstream
            if (cells[i].CurrentUnit != null)
            {
=======
            if (cells[i].CurrentUnit != null && cells[i].CurrentUnit.TeamUnit != teamUnit)
            {                
>>>>>>> Stashed changes
                cells[i].CurrentUnit.Fighter.TakeDamage(_damage);
                return true;
            }
        }

        return false;
    }

    private void TakeDamage(int damage)
    {
        _health = _health - damage >= 0 ? -damage : 0;

        if (_health == 0)
            Die();
    }

    private void Die()
    {
        Died?.Invoke(this);
    }
}
