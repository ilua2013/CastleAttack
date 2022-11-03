using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Cell))]
public class UnitSpawner : MonoBehaviour, ICardApplicable
{
    [SerializeField] private SpawnerType _type;
    [SerializeField] private Transform _spawnPoint; 

    private LevelSystem _finisher;
    private List<UnitEnemy> _enemyUnits = new List<UnitEnemy>();
    private List<UnitFriend> _friendUnits = new List<UnitFriend>();

    public Transform SpawnPoint => _spawnPoint;

    public event Action<IUnit> SpawnedUnit;

    private void OnValidate()
    {
        _spawnPoint = transform;
    }

    private void Awake()
    {
        _finisher = FindObjectOfType<LevelSystem>();
    }

    private void OnEnable()
    {
        _finisher.Wave1Finished += OnFinished;
        _finisher.Wave2Finished += OnFinished;
        _finisher.Wave3Finished += OnFinished;
    }

    private void OnDisable()
    {
        _finisher.Wave1Finished -= OnFinished;
        _finisher.Wave2Finished -= OnFinished;
        _finisher.Wave3Finished -= OnFinished;
    }

    public bool TryApplyFriend(Card card, Vector3 place)
    {
        if (_type == SpawnerType.Enemy)
            return false;

        if (card is UnitCard unitCard)
        {
            UnitFriend unitFriend = Instantiate(unitCard.UnitPrefab, SpawnPoint.position, Quaternion.identity);

            unitFriend.Init(card, GetComponent<Cell>());
            unitFriend.Fighter.Died_get += OnFriendUnitDead;

            _friendUnits.Add(unitFriend);

            SpawnedUnit?.Invoke(unitFriend);

            return true;
        }
        
        return false;
    }

    public UnitEnemy TryApplyEnemy(UnitEnemy unitEnemy)
    {
        UnitEnemy unit = Instantiate(unitEnemy, SpawnPoint.position, Quaternion.identity);

        unit.Init(unitEnemy.Card, GetComponent<Cell>());
        unit.Fighter.Died_get += OnEnemyUnitDead;

        _enemyUnits.Add(unit);
        SpawnedUnit?.Invoke(unit);

        return unit;
    }

    private void OnFinished()
    {
        foreach (var unit in _friendUnits)
        {
            unit.Card.ComeBack();
            unit.ReturnToHand();
        }

        _friendUnits.Clear();
    }

    private void OnEnemyUnitDead(Fighter fighter)
    {
        fighter.Died_get -= OnFriendUnitDead;

        if (fighter.Unit is UnitEnemy unitEnemy)
            _enemyUnits.Remove(unitEnemy);
    }

    private void OnFriendUnitDead(Fighter fighter)
    {
        fighter.Died_get -= OnFriendUnitDead;

        if (fighter.Unit is UnitFriend unitFriend)
            _friendUnits.Remove(unitFriend);
    }
}

public enum SpawnerType
{
    Friend, Enemy
}
