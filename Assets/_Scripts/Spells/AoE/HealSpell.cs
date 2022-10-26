using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HealSpell : AoESpell
{
    private SphereCollider _collider;
    private int _recovery;

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

    public void Init(int recovery)
    {
        _recovery = recovery;
    }

    protected override void Affect()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _collider.radius);

        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out IMonstr triggered))
            {
                Debug.Log("Affect " + triggered);
                triggered.RecoveryHealth(_recovery);
            }
        }
    }

    private void OnDispelled()
    {
        gameObject.SetActive(false);
    }
}
