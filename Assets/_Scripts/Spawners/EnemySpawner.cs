using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private LevelEnemiesData _levelEnemiesData;
    [SerializeField] private List<UnitSpawner> _cellsEnemySpawner;
    [Header("StartUnit")]
    [SerializeField] private UnitEnemy[] _enemysStart = new UnitEnemy[] { };

    private List<Wave> _waves = new List<Wave>();
    private int _currentWave = 0;

    public int WaveCount => _waves.Count;
    public int CurrentWave => _currentWave;
    public bool HaveWave => _waves.Count > _currentWave;

    public event Action WaveCountChanged;
    public event Action<UnitEnemy> Spawned_get;

    [Serializable]
    public class Wave
    {
        public UnitEnemy[] UnitEnemies = new UnitEnemy[] { };

        public Wave(UnitEnemy[] unitEnemies)
        {
            UnitEnemies = unitEnemies;
        }
    }

    private void OnValidate()
    {
        _battleSystem = FindObjectOfType<BattleSystem>();
        _enemysStart = GetComponentsInChildren<UnitEnemy>();

        if (_levelEnemiesData == null)
            _levelEnemiesData = Resources.Load("Configs/LevelEnemies") as LevelEnemiesData;

        //_levelEnemiesData.

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

        if (_currentWave >= _waves.Count || PossibleSpawnUnits(_waves[_currentWave].UnitEnemies.Length) == false)
            return;

        for (int i = 0; i < _waves[_currentWave].UnitEnemies.Length; i++)
        {
            UnitEnemy spawn = _waves[_currentWave].UnitEnemies[i];

            if (_battleSystem.UnitsFriend.Count == 0)
                RandomSpawn(spawn);
            else
            {
                switch (Random.Range(0, 2))
                {
                    case 0:
                        RandomSpawn(spawn);
                        break;
                    case 1:
                        SpawnOnUnitFriend(spawn);
                        break;
                }
            }
        }

        _currentWave++;

        WaveCountChanged?.Invoke();
    }

    private void SpawnOnUnitFriend(UnitEnemy unitEnemy)
    {
        List<UnitFriend> unitFriends = _battleSystem.UnitsFriend;
        Cell cell = unitFriends[Random.Range(0, unitFriends.Count)].Mover.CurrentCell;

        while (cell.Top != null)
        {
            if (cell.Top.IsFree && cell.Top.TryGetComponent(out UnitSpawner unitSpawner) && unitSpawner.SpawnerType == SpawnerType.Enemy)
            {
                Spawn(unitSpawner, unitEnemy);
                return;
            }
            else
                cell = cell.Top;
        }

        RandomSpawn(unitEnemy);
    }

    private void RandomSpawn(UnitEnemy unitEnemy)
    {
        UnitSpawner spawner = null;
        int indexSpawner = Random.Range(0, _cellsEnemySpawner.Count);

        for (int a = 0; a < _cellsEnemySpawner.Count; a++)
        {
            if (_cellsEnemySpawner[indexSpawner].TryGetComponent(out Cell cell) && cell.IsFree == true)
                spawner = _cellsEnemySpawner[indexSpawner];

            indexSpawner = Random.Range(0, _cellsEnemySpawner.Count);
        }

        if (spawner != null)
            Spawn(spawner, unitEnemy);
    }

    private void Spawn(UnitSpawner enemySpawner, UnitEnemy unitStep)
    {
        var spawned = enemySpawner.TryApplyEnemy(unitStep);

        Spawned_get?.Invoke(spawned);
    }

    private bool PossibleSpawnUnits(int countUnit)
    {
        int countFreeCell = 0;

        foreach (var item in _cellsEnemySpawner)
        {
            if (item.Cell.IsFree)
                countFreeCell++;
        }

        return countFreeCell >= countUnit ? true : false;
    }
}
