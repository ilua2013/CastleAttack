using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class UnitPrefab
{
    [SerializeField] private CardStage _stage;
    [SerializeField] private UnitFriend _prefab;

    public CardStage Stage => _stage;
    public UnitFriend Prefab => _prefab;
}
