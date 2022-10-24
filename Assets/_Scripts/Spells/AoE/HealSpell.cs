using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : AoESpell
{
    private float _recovery;

    private void OnEnable()
    {
        Dispelled += OnDispelled;
    }

    private void OnDisable()
    {
        Dispelled -= OnDispelled;
    }

    public void Init(float recovery)
    {
        _recovery = recovery;
    }

    protected override void Affect()
    {

    }

    private void OnDispelled()
    {
        gameObject.SetActive(false);
    }
}
