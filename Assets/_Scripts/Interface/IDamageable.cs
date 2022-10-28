using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    public int Health { get; }
    public int MaxHealth { get; }
    public void TakeDamage(int damage);

    public event UnityAction<IDamageable, int> Damaged;
    public event UnityAction<IDamageable, int> Healed;
}
