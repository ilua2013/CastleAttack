using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Cell))]
public class UnitSpawner : MonoBehaviour, ICardApplicable
{
    [SerializeField] private SpawnerType _type;
    [SerializeField] private SoundEffectPlayer _sounds;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Cell _cell;

    public UnitFriend Spawned { get; private set; }
    public Vector3 SpawnPoint => _spawnPoint.position;
    public SpawnerType SpawnerType => _type;
    public Cell Cell => _cell;

    public event Action<IUnit> SpawnedUnit_get;
    public event Action SpawnedUnit;

    private void OnValidate()
    {
        _cell = GetComponent<Cell>();
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

            SpawnedUnit_get?.Invoke(unitFriend);
            SpawnedUnit?.Invoke();
            _sounds.Play(SoundEffectType.Spawn);

            return true;
        }
        
        return false;
    }

    public UnitEnemy TryApplyEnemy(UnitEnemy unitEnemy)
    {
        UnitEnemy unit = Instantiate(unitEnemy, SpawnPoint, Quaternion.Euler(0,180,0));

        unit.Init(unitEnemy.Card, _cell);

        SpawnedUnit_get?.Invoke(unit);
        SpawnedUnit?.Invoke();

        return unit;
    }
}

public enum SpawnerType
{
    Friend, Enemy
}
