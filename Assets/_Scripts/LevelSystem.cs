using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Agava.YandexMetrica;
using UnityEngine.SceneManagement;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private UnitFriend _wizzard;
    [SerializeField] private BattleSystem _battleSystem;
    [Header("Wave 1")]
    [SerializeField] private EnemySpawner _enemySpawner1;
    [Header("Wave 2")]
    [SerializeField] private EnemySpawner _enemySpawner2;
    [Header("Wave 3")]
    [SerializeField] private EnemySpawner _enemySpawner3;

    private StageNumber _currentWave;

    public StageNumber CurrentWave => _currentWave;

    public UnitFriend Wizzard => _wizzard;

    public event Action WaveFinished;
    public event Action Wave1Finished;
    public event Action Wave2Finished;
    public event Action Wave3Finished;
    public event Action Failed;

    private void OnValidate()
    {
        _battleSystem = FindObjectOfType<BattleSystem>();
        if (_wizzard != null && _wizzard.Fighter.FighterType != FighterType.MainWizzard)
            _wizzard = null;
    }

    private void Start()
    {
        _currentWave = StageNumber.One;
        _enemySpawner1.Init();
    }

    private void OnEnable()
    {
        _battleSystem.StepFinished += CheckWinWave;
        _wizzard.Fighter.ReadyToDie += Fail;
    }

    private void OnDisable()
    {
        _battleSystem.StepFinished -= CheckWinWave;
        _wizzard.Fighter.ReadyToDie -= Fail;
    }

    private void CheckWinWave()
    {
        if (_battleSystem.CountEnemy != 0)
            return;

        switch (_currentWave)
        {
            case StageNumber.One:
                if (_enemySpawner1.HaveWave == false)
                {
                    _currentWave++;

                    _battleSystem.StopDoStep();
                    _battleSystem.ReturnToHandFriend();
                    _battleSystem.SetEnemySpawner(_enemySpawner2);

                    _enemySpawner2.Init();

                    Wave1Finished?.Invoke();
                    WaveFinished?.Invoke();
                }
                break;

            case StageNumber.Two:
                if (_enemySpawner2.HaveWave == false)
                {
                    _currentWave++;

                    _battleSystem.StopDoStep();
                    _battleSystem.ReturnToHandFriend();
                    _battleSystem.SetEnemySpawner(_enemySpawner3);

                    _enemySpawner3.Init();

                    Wave2Finished?.Invoke();
                    WaveFinished?.Invoke();
                }
                break;

            case StageNumber.Three:
                if (_enemySpawner3.HaveWave == false)
                {
                    _battleSystem.StopDoStep();

                    Wave3Finished?.Invoke();
                    WaveFinished?.Invoke();
                }
                break;
        }
    }

    private void Fail()
    {
        YandexMetrica.Send("Fail", new Dictionary<string, string>() { { "Level", $"{SceneManager.GetActiveScene().buildIndex - 2}" } });
        Failed?.Invoke();
    }
}

public enum StageNumber
{
    One, Two, Three
}
