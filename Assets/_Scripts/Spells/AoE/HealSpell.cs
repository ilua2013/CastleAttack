using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpell : Spell
{
    [SerializeField] private int _recovery;

    public int Recovery => _recovery;

    private void OnEnable()
    {
        Dispelled += OnDispelled;
    }

    private void OnDisable()
    {
        Dispelled -= OnDispelled;
    }

    protected override void Affect(Cell cell, CardSave save)
    {
        List<UnitFriend> units = cell.GetFriendUnits(DistanceAttacks);

        foreach (UnitFriend unit in units)
            unit.Fighter.RecoveryHealth(save.UnitStats.MaxHealth);
    }

    private void OnDispelled()
    {
        gameObject.SetActive(false);
    }
}
