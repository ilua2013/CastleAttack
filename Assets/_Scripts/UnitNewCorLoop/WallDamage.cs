using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDamage : MonoBehaviour
{
    [SerializeField] private List<Fighter> _moverOnCellStopeds;
    [SerializeField] private int _maxHealth = 100;

    private int _health;

    public event Action StopedGame;

    private void Awake()
    {
        _health = _maxHealth;
    }

    private void OnEnable()
    {
        foreach (var pieceOfWall in _moverOnCellStopeds)
        {
            pieceOfWall.Damaged += RemoveHealt;
        }
    }

    private void OnDisable()
    {
        foreach (var pieceOfWall in _moverOnCellStopeds)
        {
            pieceOfWall.Damaged -= RemoveHealt;
        }
    }

    private void RemoveHealt(int damage)
    {
        Debug.Log(damage);
        _health = _health-damage;
        if (_health < 0)
        {
            StopedGame?.Invoke();
        }
    }
}
