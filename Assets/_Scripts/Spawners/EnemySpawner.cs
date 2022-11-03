using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private UnitEnemy _mainTarget;
    [SerializeField] private int _waveCount;
    [SerializeField] private List<UnitSpawner> _cellsEnemySpawner;
    [SerializeField] private List<UnitEnemy> _enemyUnitsPrefab;
    [SerializeField] private FightSystem _fightSystem;
    [Header("Add Params")]
    [SerializeField] private int _minusWaveOnDieBuild = 3;
    [Header("StartSpawn (index 1 to 1)")]
    [SerializeField] private UnitSpawner[] _spawnersStart;
    [SerializeField] private UnitEnemy[] _enemysStart;

    private int _currentWave = 0;

    public int WaveCount => _waveCount;
    public int CurrentWave => _currentWave;

    public bool HaveWave => _waveCount > _currentWave;

    public event Action WaveCountChanged;
    public event Action<UnitEnemy> Spawned_get;

    private void OnValidate()
    {
        if (_spawnersStart.Length != _enemysStart.Length)
            Debug.LogError("_cells.Length != _enemys.Length !");
    }

    private void Start()
    {
        SpawnOnStart();
    }

    private void OnEnable()
    {
        _fightSystem.StepFinished += EnemySpawn;

        if (_mainTarget != null)
            _mainTarget.Fighter.Died_get += OnDieMainTarget;
    }
    private void OnDisable()
    {
        _fightSystem.StepFinished -= EnemySpawn;
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
        if (_currentWave >= _waveCount)
            return;

        Spawn(_cellsEnemySpawner[Random.Range(0, _cellsEnemySpawner.Count)], _enemyUnitsPrefab[Random.Range(0, _enemyUnitsPrefab.Count)]);
        _currentWave++;

        WaveCountChanged?.Invoke();
    }

    private void SpawnOnStart()
    {
        for (int i = 0; i < _spawnersStart.Length; i++)
            Spawn(_spawnersStart[i], _enemysStart[i]);
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
