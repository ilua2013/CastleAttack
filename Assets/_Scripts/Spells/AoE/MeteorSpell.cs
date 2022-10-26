using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MeteorSpell : AoESpell
{
    private int _damage;
    private SphereCollider _collider;

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

    public void Init(int damage)
    {
        _damage = damage;
    }

    protected override void Affect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _collider.radius);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out IMob triggered))
            {
                Debug.Log("Affect " + triggered);
                triggered.TakeDamage(_damage);
            }
        }
    }

    private void OnDispelled()
    {
        gameObject.SetActive(false);
    }
}
