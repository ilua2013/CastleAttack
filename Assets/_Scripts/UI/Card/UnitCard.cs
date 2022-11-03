using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : Card
{
    [SerializeField] private UnitFriend _unitPrefab;    

    public UnitFriend UnitPrefab => _unitPrefab;   
}
