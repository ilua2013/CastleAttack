using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProjection : Projection
{
    [SerializeField] private RadiusAttackView _radiusAttack;

    private Cell _cell;

    public override void Init(Cell cell)
    {
        _cell = cell;
    }

    public override void Show(Cell cell)
    {
        _radiusAttack.ShowRadius(cell);
    }
}
