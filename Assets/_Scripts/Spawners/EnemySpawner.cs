using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private int _waveCount;
    [SerializeField] private int _countSpawnOneWave;
    [SerializeField] private List<UnitSpawner> _cellsEnemySpawner;
    [SerializeField] private List<UnitEnemy> _enemyUnitsPrefab;
    [Header("StartUnit")]
    [SerializeField] private UnitEnemy[] _enemysStart = new UnitEnemy[] { };

    private int _currentWave = 0;

    public int WaveCount => _waveCount;
    public int CurrentWave => _currentWave;
    public bool HaveWave => _waveCount > _currentWave;

    public event Action WaveCountChanged;
    public event Action<UnitEnemy> Spawned_get;

    private void OnValidate()
    {
        _battleSystem = FindObjectOfType<BattleSystem>();
        _enemysStart = GetComponentsInChildren<UnitEnemy>();

        if (_enemysStart != null)
        {
            for (int i = 0; i < _enemysStart.Length; i++)
                if (_enemysStart[i].transform.parent != transform)
                    _enemysStart[i].transform.parent = transform;
        }

        _cellsEnemySpawner.Clear();

        foreach (var item in FindObjectOfType<CellSpawner>().GetComponentsInChildren<UnitSpawner>())
        {
            if (item.SpawnerType == SpawnerType.Enemy && item.TryGetComponent(out Cell cell) && cell.CellIs != CellIs.Boss && _cellsEnemySpawner.Contains(item) == false)
                _cellsEnemySpawner.Add(item);
        }
    }

    private void Start()
    {
        Init();
    }

    private void OnDisable()
    {
        _battleSystem.StepFinished -= EnemySpawn;
    }

    public void Init()
    {
        _battleSystem.StepFinished += EnemySpawn;

        foreach (var item in _enemysStart)
        {
            item.Init(null, null);
            Spawned_get?.Invoke(item);
        }
    }

    public void TutorialEnemy(UnitEnemy[] unitEnemies)
    {
        _enemysStart = unitEnemies;
        Init();
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
        for (int i = 0; i < _countSpawnOneWave; i++)
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

        Spawned_get?.Invoke(spawned);
    }
}
