using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjection : Projection, IRadiusAttack
{
    [SerializeField] private DistanceAttack[] _distanceAttacks;
    [SerializeField] private RadiusAttackView _radiusAttack;

    private Cell _cell;

    public Mover Mover => null;

    public DistanceAttack[] DistanceAttack => _distanceAttacks;

    public event Action Inited;

    public override void Init(Cell cell)
    {
        _cell = cell;
    }

    public List<Cell> RadiusView()
    {
        return _cell.GetCellsDistanceAttack(_distanceAttacks);
    }

    public override void Show(Cell cell)
    {
        _radiusAttack.ShowRadius(cell, _distanceAttacks);
    }
}
