using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPrioretyComparer : IComparer<UnitEnemy>
{
    private UnitEnemy _first;

    public FirstPrioretyComparer(UnitEnemy first)
    {
        _first = first;
    }

    public int Compare(UnitEnemy x, UnitEnemy y)
    {
        if (x == _first)
            return 1;

        if (y == _first)
            return -1;

        return 0;
    }
}
