using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell : AoESpell
{
    private float _damage;

    private void OnEnable()
    {
        Dispelled += OnDispelled;
    }

    private void OnDisable()
    {
        Dispelled -= OnDispelled;
    }

    public void Init(float damage)
    {
        _damage = damage;
    }

    protected override void Affect()
    {

    }

    private void OnDispelled()
    {
        gameObject.SetActive(false);
    }
}
