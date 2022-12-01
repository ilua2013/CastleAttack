using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private UnitEnemy _mainTarget;
    [SerializeField] private int _countSpawnWave;
    [SerializeField] private int _waveCount;
    [SerializeField] private List<UnitSpawner> _cellsEnemySpawner;
    [SerializeField] private List<UnitEnemy> _enemyUnitsPrefab;
    [SerializeField] private BattleSystem _battleSystem;
    [Header("Add Params")]
    [SerializeField] private int _minusWaveOnDieBuild = 3;
    [Header("StartUnit")]
    [SerializeField] private UnitEnemy[] _enemysStart = new UnitEnemy[50];

    private int _currentWave = 0;

    public int WaveCount => _waveCount;
    public int CurrentWave => _currentWave;
    public bool HaveWave => _waveCount > _currentWave;

    public event Action WaveCountChanged;
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
            if (item.Fighter.FighterType == FighterType.Build)
                item.Fighter.Died_get += OnDieBuild;
        }
    }

    public void TutorialEnemy(UnitEnemy[] unitEnemies)
    {
        _enemysStart = unitEnemies;
        Init();
    }

    public void MinusWaveCount(int value)
    {
        _waveCount = _waveCount - value < 0 ? 0 : _waveCount - value;
        _currentWave = _currentWave > _waveCount ? _waveCount : _currentWave;

        WaveCountChanged?.Invoke();
    }

    private void OnDieMainTarget(Fighter fighter)
    {
        fighter.Died_get -= OnDieMainTarget;
        MinusWaveCount(_waveCount);
    }

    private void EnemySpawn()
    {
        if (_cellsEnemySpawner.Count == 0)
        {
            _currentWave++;
            WaveCountChanged?.Invoke();
            return;
        }

        if (_currentWave >= _waveCount)
            return;

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

        _currentWave++;

        WaveCountChanged?.Invoke();
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
        MinusWaveCount(_minusWaveOnDieBuild);
    }
}
