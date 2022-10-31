using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : Card
{
    [SerializeField] private UnitStep _unitPrefab;    

    public UnitStep UnitPrefab => _unitPrefab;   
}
