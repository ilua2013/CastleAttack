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

    private List<UnitStep> _enemyUnits = new List<UnitStep>();

    private LevelSystem _finisher;
    private List<UnitStep> _units = new List<UnitStep>();

    public Transform SpawnPoint => _spawnPoint;

    public event Action<UnitStep> SpawnedUnit;

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

    public bool TryApply(Card card, Vector3 place)
    {
        if (_type == SpawnerType.Enemy)
            return false;

        if (card is UnitCard unitCard)
        {
            UnitStep unit = Instantiate(unitCard.UnitPrefab, SpawnPoint.position, Quaternion.identity);

            unit.Init(card, GetComponent<Cell>(), TeamUnit.Friend);
            unit.Fighter.Died += OnUnitDead;

            _units.Add(unit);

            SpawnedUnit?.Invoke(unit);

            return true;
        }
        
        return false;
    }

    public void TryApplyEnemy(UnitStep unitStep)
    {
        UnitStep unit = Instantiate(unitStep, SpawnPoint.position, Quaternion.identity);
        unit.Init(unitStep.Card, GetComponent<Cell>(), TeamUnit.Enemy);
        unit.Fighter.Died += OnEnemyUnitDead;
        _enemyUnits.Add(unit);
        SpawnedUnit?.Invoke(unit);      
    }

    private void OnFinished()
    {
        foreach (var unit in _units)
        {
            unit.Card.ComeBack();
            unit.ReturnToHand();
        }

        _units.Clear();
    }

    private void OnEnemyUnitDead(Fighter fighter)
    {
        fighter.Died -= OnUnitDead;
        _enemyUnits.Remove(fighter.GetComponent<UnitStep>());
    }

    private void OnUnitDead(Fighter fighter)
    {
        fighter.Died -= OnUnitDead;
        _units.Remove(fighter.GetComponent<UnitStep>());
    }
}

public enum SpawnerType
{
    Friend, Enemy
}
