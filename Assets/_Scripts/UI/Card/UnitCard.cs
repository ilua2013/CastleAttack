using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitCard : Card
{
    [SerializeField] private List<UnitPrefab> _unitPrefabs;

    public UnitFriend UnitPrefab => _unitPrefabs.First((prefab) => prefab.Stage == Stage).Prefab;
}
