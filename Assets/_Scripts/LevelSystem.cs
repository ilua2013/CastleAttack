using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private Button _buttonStartFight;
    [SerializeField] private Button _buttonStartFight2;
    [SerializeField] private Button _buttonStartFight3;
    [Header("Wave 1")]
    [SerializeField] private BattleSystem _fightSystem1;
    [SerializeField] private EnemySpawner _enemySpawner1;
    //private List<UnitStep> _building = new List<UnitStep>();

    [Header("Wave 2")]
    [SerializeField] private BattleSystem _fightSystem2;
    [SerializeField] private EnemySpawner _enemySpawner2;
    [SerializeField] private UnitEnemy _castleDoor;

    [Header("Wave 3")]
    [SerializeField] private BattleSystem _fightSystem3;
    [SerializeField] private EnemySpawner _enemySpawner3;
    [SerializeField] private UnitEnemy _boss;

    private Wave _currentWave;

    public Wave CurrentWave => _currentWave;

    public event Action ClickedStartFight;
    public event Action Wave1Finished;
    public event Action Wave2Finished;
    public event Action Wave3Finished;

    private void OnValidate()
    {
        //var build = FindObjectsOfType<UnitStep>();
        //_building.Clear();

        //foreach (var item in build)
        //{
        //    if (item.Fighter.FighterType == FighterType.Build)
        //        _building.Add(item);
        //}
    }

    private void Awake()
    {
        _currentWave = Wave.One;

        _buttonStartFight2.gameObject.SetActive(false);
        _buttonStartFight3.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _buttonStartFight.onClick.AddListener(StartFight);

        //_enemySpawner1.Spawned_get += TryAddBuilding;

        //_enemySpawner1.Spawned_get += RegisterDie;
        //_enemySpawner2.Spawned_get += RegisterDie;
        //_enemySpawner3.Spawned_get += RegisterDie;
    }

    private void OnDisable()
    {
        _buttonStartFight.onClick.RemoveListener(StartFight);

        //_enemySpawner1.Spawned_get -= TryAddBuilding;

        //_enemySpawner1.Spawned_get -= RegisterDie;
        //_enemySpawner2.Spawned_get -= RegisterDie;
        //_enemySpawner3.Spawned_get -= RegisterDie;
    }

    private void StartFight()
    {
        ClickedStartFight?.Invoke();
    }

    private void TryAddBuilding(UnitEnemy unitStep)
    {
        //print(unitStep == null);
        //if (_building.Contains(unitStep) == true || unitStep.Fighter.FighterType != FighterType.Build)
        //    return;

        //_building.Add(unitStep);
    }

    private void RemoveDiedUnit(Fighter fighter)
    {
        switch (_currentWave)
        {
            case Wave.One:
                //RemoveUnit(_building, fighter.GetComponent<UnitStep>());
                //RemoveUnit(fighter.GetComponent<UnitStep>());
                break;

            case Wave.Two:
                //RemoveUnit(fighter.GetComponent<UnitStep>());
                break;

            case Wave.Three:
               // RemoveUnit(fighter.GetComponent<UnitStep>());
                break;
        }

        CheckWinWave();
    }

    private void CheckWinWave()
    {
        switch (_currentWave)
        {
            case Wave.One:
                if(_fightSystem1.CountEnemy == 0 && _enemySpawner1.HaveWave == false)
                {
                    _currentWave++;
                    _fightSystem1.StopDoStep();

                    _buttonStartFight.gameObject.SetActive(false);
                    _buttonStartFight2.gameObject.SetActive(true);
                    _buttonStartFight3.gameObject.SetActive(false);

                    Wave1Finished?.Invoke();
                    Debug.Log("Finished 1");
                }
                break;

            case Wave.Two:
                if (_castleDoor.Fighter.IsDead == true && _enemySpawner2.HaveWave == false && _fightSystem2.CountEnemy == 0)
                {
                    _currentWave++;
                    _fightSystem2.StopDoStep();

                    _buttonStartFight.gameObject.SetActive(false);
                    _buttonStartFight2.gameObject.SetActive(false);
                    _buttonStartFight3.gameObject.SetActive(true);

                    Wave2Finished?.Invoke();
                    Debug.Log("Finished 2");
                }
                break;

            case Wave.Three:
                if (_boss.Fighter.IsDead == true && _fightSystem3.CountEnemy == 0 && _enemySpawner3.HaveWave == false)
                {
                    _fightSystem3.StopDoStep();

                    Wave3Finished?.Invoke();
                    Debug.Log("Finished 3");
                }
                break;
        }
    }

    private void RemoveUnit(List<UnitEnemy> units, UnitEnemy unitStep)
    {
        if (units.Contains(unitStep))
        {
            units.Remove(unitStep);
            unitStep.Fighter.Died_get -= RemoveDiedUnit;
        }
    }

    private void RemoveUnit(UnitEnemy unitStep)
    {
        unitStep.Fighter.Died_get -= RemoveDiedUnit;
    }

    private void RegisterDie(List<UnitEnemy> units)
    {
        foreach (var mob in units)
            mob.Fighter.Died_get += RemoveDiedUnit;
    }

    private void RegisterDie(UnitEnemy unitStep)
    {
        unitStep.Fighter.Died_get += RemoveDiedUnit;
    }
}

public enum Wave
{
    One, Two, Three
}
