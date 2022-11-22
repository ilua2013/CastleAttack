using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Cell))]
public class UnitSpawner : MonoBehaviour, ICardApplicable
{
    [SerializeField] private SpawnerType _type;
    [SerializeField] private Transform _spawnPoint;

    private Cell _cell;

    public UnitFriend Spawned { get; private set; }
    public Vector3 SpawnPoint => _spawnPoint.position;

    public Cell Cell => _cell;

    public event Action<IUnit> SpawnedUnit;

    private void Awake()
    {
        _cell = GetComponent<Cell>();
    }

    private void OnValidate()
    {
        _spawnPoint = transform;
    }

    public bool CanApply(Card card)
    {
        if (card is UnitCard unitCard)
            return _cell.IsFree && _type != SpawnerType.Enemy;

        return false;
    }

    public bool TryApplyFriend(Card card, Vector3 place)
    {
        if (_type == SpawnerType.Enemy || _cell.IsFree == false)
            return false;

        if (card is UnitCard unitCard)
        {
            UnitFriend unitFriend = Instantiate(unitCard.UnitPrefab, SpawnPoint, Quaternion.identity);
            Spawned = unitFriend;

            unitFriend.Init(unitCard, _cell);

            SpawnedUnit?.Invoke(unitFriend);

            return true;
        }
        
        return false;
    }

    public UnitEnemy TryApplyEnemy(UnitEnemy unitEnemy)
    {
        UnitEnemy unit = Instantiate(unitEnemy, SpawnPoint, Quaternion.identity);

        unit.Init(unitEnemy.Card, _cell);

        SpawnedUnit?.Invoke(unit);

        return unit;
    }
}

public enum SpawnerType
{
    Friend, Enemy
}
