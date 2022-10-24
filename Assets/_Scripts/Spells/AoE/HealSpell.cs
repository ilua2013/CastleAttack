using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : AoESpell
{
    [SerializeField] private float _recoveryPerSecond;

    private void OnEnable()
    {
        Dispelled += OnDispelled;
    }

    private void OnDisable()
    {
        Dispelled -= OnDispelled;
    }

    private void OnDispelled()
    {

    }
}
