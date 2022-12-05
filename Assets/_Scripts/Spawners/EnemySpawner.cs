using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private UnitEnemy _mainTarget;
    [SerializeField] private int _countSpawnWave;
    [SerializeField] private List<UnitSpawner> _cellsEnemySpawner;
    [SerializeField] private List<UnitEnemy> _enemyUnitsPrefab;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private List<UnitEnemy> _targetEnemy = new List<UnitEnemy>();
    [Header("StartUnit")]
    [SerializeField] private UnitEnemy[] _enemysStart = new UnitEnemy[20];

    public List<UnitEnemy> TargetEnemy => _targetEnemy;
    public UnitEnemy[] EnemyStart => _enemysStart;
    public UnitEnemy MainTarget => _mainTarget;

    public event Action WaveCountChanged;
    public event Action DiedBuild;
    public event Action DiedTarget;
    public event Action<UnitEnemy> Spawned_get;

    private void OnValidate()
    {
        _battleSystem = FindObjectOfType<BattleSystem>();

        if (_enemysStart != null)
        {
            for (int i = 0; i < _enemysStart.Length; i++)
                if (_enemysStart[i].transform.parent != transform)
                    _enemysStart[i].transform.parent = transform;
        }

        _enemysStart = GetComponentsInChildren<UnitEnemy>();
        _cellsEnemySpawner.Clear();

        foreach (var item in GetComponentInParent<Stage>().CellSpawner.GetComponentsInChildren<UnitSpawner>())
        {
            if (item.SpawnerType == SpawnerType.Enemy && item.TryGetComponent(out Cell cell) && cell.CellIs != CellIs.Boss && _cellsEnemySpawner.Contains(item) == false)
                _cellsEnemySpawner.Add(item);
        }
    }

    private void OnEnable()
    {
        if (_mainTarget != null)
            _mainTarget.Fighter.Died_get += OnDieMainTarget;
    }

    private void OnDisable()
    {
        _battleSystem.StepFinished -= EnemySpawn;

        if (_mainTarget != null)
            _mainTarget.Fighter.Died_get -= OnDieMainTarget;
    }

    public void Init()
    {
        _battleSystem.StepFinished += EnemySpawn;

        foreach (var item in _enemysStart)
        {
            item.Init(null, null);
            Spawned_get?.Invoke(item);
        }

        foreach (var item in _targetEnemy)
        {
            item.Init(null, null);
            Spawned_get?.Invoke(item);

            item.Fighter.Died_get += RemoveDiedUnit;
        }
    }

    public void RemoveDiedUnit(Fighter fighter)
    {
        if (_targetEnemy.Contains((UnitEnemy)fighter.Unit))
        {
            _targetEnemy.Remove((UnitEnemy)fighter.Unit);
            DiedTarget?.Invoke();
            fighter.Died_get -= RemoveDiedUnit;
        }
    }

    public void TutorialEnemy(UnitEnemy[] unitEnemies)
    {
        _enemysStart = unitEnemies;
        Init();
    }

    public int GetBuildCount()
    {
        int i = 0;

        foreach (var item in _enemysStart)
        {
            if (item.Fighter.FighterType == FighterType.Build && item.Fighter.IsDead == false)
                i++;
        }
        print(i + " s");
        return i;
    }

    public int GetBossCount()
    {
        int i = 0;

        foreach (var item in _enemysStart)
        {
            if (item.Fighter.FighterType == FighterType.MainTarget && item.Fighter.IsDead == false)
                i++;
        }

        return i;
    }

    private void OnDieMainTarget(Fighter fighter)
    {
        fighter.Died_get -= OnDieMainTarget;
    }

    private void EnemySpawn()
    {
        if (_cellsEnemySpawner.Count == 0 || _targetEnemy.Count == 0)
        {
            WaveCountChanged?.Invoke();
            return;
        }

        if (_battleSystem.UnitsFriend.Count == 0)
            RandomSpawn();
        else
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    RandomSpawn();
                    break;
                case 1:
                    SpawnOnUnitFriend();
                    break;
            }
        }
    }

    private void SpawnOnUnitFriend()
    {
        List<UnitFriend> unitFriends = _battleSystem.UnitsFriend;
        Cell cell = unitFriends[Random.Range(0, unitFriends.Count)].Mover.CurrentCell;

        while (cell.Top != null)
        {
            if (cell.Top.IsFree && cell.Top.TryGetComponent(out UnitSpawner unitSpawner) && unitSpawner.SpawnerType == SpawnerType.Enemy)
            {
                Spawn(unitSpawner, _enemyUnitsPrefab[Random.Range(0, _enemyUnitsPrefab.Count)]);
                break;
            }
            else
                cell = cell.Top;
        }
    }

    private void RandomSpawn()
    {
        for (int i = 0; i < _countSpawnWave; i++)
        {
            UnitSpawner spawner = null;
            int indexSpawner = Random.Range(0, _cellsEnemySpawner.Count);
            int indexUnit = Random.Range(0, _enemyUnitsPrefab.Count);

            for (int a = 0; a < _cellsEnemySpawner.Count; a++)
            {
                if (_cellsEnemySpawner[indexSpawner].TryGetComponent(out Cell cell) && cell.IsFree == true)
                    spawner = _cellsEnemySpawner[indexSpawner];

                indexSpawner = Random.Range(0, _cellsEnemySpawner.Count);
            }

            if (spawner != null)
                Spawn(spawner, _enemyUnitsPrefab[indexUnit]);
        }
    }

    private void Spawn(UnitSpawner enemySpawner, UnitEnemy unitStep)
    {
        var spawned = enemySpawner.TryApplyEnemy(unitStep);

        if (spawned.Fighter.FighterType == FighterType.Build)
            spawned.Fighter.Died_get += OnDieBuild;

        Spawned_get?.Invoke(spawned);
    }

    private void OnDieBuild(Fighter fighter)
    {
        fighter.Died_get -= OnDieBuild;

        DiedBuild?.Invoke();
    }
}
