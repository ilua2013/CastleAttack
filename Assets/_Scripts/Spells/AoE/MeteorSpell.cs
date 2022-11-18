using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MeteorSpell : Spell
{
    [SerializeField] private int _damage;

    private BoxCollider _collider;

    public int Damage => _damage;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        Dispelled += OnDispelled;
    }

    private void OnDisable()
    {
        Dispelled -= OnDispelled;
    }

    protected override void Affect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _collider.size.x);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out UnitEnemy enemy))
            {
                Debug.Log("Affect " + enemy);
                enemy.Fighter.TakeDamage(_damage);
            }
        }
    }

    private void OnDispelled()
    {
        gameObject.SetActive(false);
    }
}
