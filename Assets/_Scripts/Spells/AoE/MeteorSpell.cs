using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MeteorSpell : Spell
{
    [SerializeField] private int _damage;

    private SphereCollider _collider;

    public int Damage => _damage;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _collider.radius);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out NewUnitEnemy enemy))
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
