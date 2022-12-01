using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UnitStats
{
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public int MaxHealth { get; private set; }

    public void Improve(int damage, int health)
    {
        Damage = damage;
        MaxHealth = health;
    }
}
