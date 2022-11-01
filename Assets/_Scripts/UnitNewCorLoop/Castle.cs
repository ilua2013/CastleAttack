using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Castle : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private int _health;

    public int Health => _health;

    public event Action Damaged;
    public event Action Died;

    private void Awake()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _health = _health - damage > 0 ? -damage : 0;

        Damaged?.Invoke();

        if (_health == 0)
            Died?.Invoke();
    }
}
