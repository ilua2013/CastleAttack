using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Cell))]
public class UnitSpawner : MonoBehaviour, ICardApplicable
{
    [SerializeField] private SpawnerType _type;
    [SerializeField] private Transform _spawnPoint; 

    public Transform SpawnPoint => _spawnPoint;
    public UnitFriend Spawned { get; private set; }

    public event Action<IUnit> SpawnedUnit;

    private void OnValidate()
    {
        _spawnPoint = transform;
    }

    public bool TryApplyFriend(Card card, Vector3 place)
    {
        if (_type == SpawnerType.Enemy)
            return false;

        if (card is UnitCard unitCard)
        {
            UnitFriend unitFriend = Instantiate(unitCard.UnitPrefab, SpawnPoint.position, Quaternion.identity);
            Spawned = unitFriend;

            unitFriend.Init(card, GetComponent<Cell>());

            SpawnedUnit?.Invoke(unitFriend);

            return true;
        }
        
        return false;
    }

    public UnitEnemy TryApplyEnemy(UnitEnemy unitEnemy)
    {
        UnitEnemy unit = Instantiate(unitEnemy, SpawnPoint.position, Quaternion.identity);

        unit.Init(unitEnemy.Card, GetComponent<Cell>());

        SpawnedUnit?.Invoke(unit);

        return unit;
    }
}

public enum SpawnerType
{
    Friend, Enemy
}
