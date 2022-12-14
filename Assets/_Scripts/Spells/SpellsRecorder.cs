using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellsRecorder : MonoBehaviour
{
    private SpellSpawner[] _spellSpawners;

    public int ActiveSpells { get; private set; }

    public event Action<Spell> WasSpellCast;

    private void Awake()
    {
        _spellSpawners = GetComponentsInChildren<SpellSpawner>();
    }

    private void OnEnable()
    {
        foreach (SpellSpawner spawner in _spellSpawners)
        {
            spawner.Cast_get += OnCast;
            spawner.Dispelled += OnDispelled;
        }
    }

    private void OnDisable()
    {
        foreach (SpellSpawner spawner in _spellSpawners)
        {
            spawner.Cast_get -= OnCast;
            spawner.Dispelled -= OnDispelled;
        }
    }

    private void OnCast(Vector3 point, Spell spell)
    {
        if (spell.ValidWhenApplied)
            return;

        ActiveSpells++;
        WasSpellCast?.Invoke(spell);
    }

    private void OnDispelled(Spell spell)
    {
        if (spell.ValidWhenApplied)
            return;

        ActiveSpells--;
    }
}
