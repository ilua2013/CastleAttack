using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CardCost
{
    [SerializeField] private int[] _costs;

    public int GetCost(CardSave save)
    {
        if (_costs.Length > save.Amount)
            return _costs[save.Amount];
        else
            return -1;
    }
}
