using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : Card
{
    [SerializeField] private Unit _unitPrefab;

    public Unit UnitPrefab => _unitPrefab;
}
