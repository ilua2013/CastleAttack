using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : Card
{
    [SerializeField] private Spell _spellPrefab;
    [SerializeField] private SpellProjection _projectionPrefab;

    public event Action<int> AmountChanged;
    public Spell SpellPrefab => _spellPrefab;

    public override Projection ProjectionPrefab => _projectionPrefab;

    public void Merge()
    {
        Amount++;
        AmountChanged?.Invoke(Amount);
    }
}
