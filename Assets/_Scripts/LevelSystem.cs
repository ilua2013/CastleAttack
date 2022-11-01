using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private Button _buttonStartFight;
    [Header("Wave 1")]
    [SerializeField] private List<UnitStep> _building;
    [Header("Wave 2")]
    [SerializeField] private UnitStep _castleDoor;
    [Header("Wave 3")]
    [SerializeField] private UnitStep _boss;

    private Wave _currentWave;

    public Wave CurrentWave => _currentWave;

    public event Action ClickedStartFight;
    public event Action Wave1Finished;
    public event Action Wave2Finished;
    public event Action Wave3Finished;

    private void OnValidate()
    {
        var build = FindObjectsOfType<UnitStep>();
        _building.Clear();

        foreach (var item in build)
        {
            if (item.Fighter.FighterType == FighterType.Build)
                _building.Add(item);
        }
    }

    private void Awake()
    {
        _currentWave = Wave.One;
    }

    private void OnEnable()
    {
        _buttonStartFight.onClick.AddListener(StartFight);
        RegisterDie(_building);
        RegisterDie(_castleDoor);
        RegisterDie(_boss);
    }

    private void OnDisable()
    {
        _buttonStartFight.onClick.RemoveListener(StartFight);
    }

    private void StartFight()
    {
        ClickedStartFight?.Invoke();
    }

    private void RemoveDiedUnit(Fighter fighter)
    {
        switch (_currentWave)
        {
            case Wave.One:
                RemoveUnit(_building, fighter.GetComponent<UnitStep>());
                break;

            case Wave.Two:
                RemoveUnit(fighter.GetComponent<UnitStep>());
                break;

            case Wave.Three:
                RemoveUnit(fighter.GetComponent<UnitStep>());
                break;
        }

        CheckWinWave();
    }

    private void CheckWinWave()
    {
        switch (_currentWave)
        {
            case Wave.One:
                if(_building.Count == 0)
                {
                    _currentWave++;
                    Wave1Finished?.Invoke();
                    Debug.Log("Finished 1");
                }
                break;

            case Wave.Two:
                if (_castleDoor.Fighter.IsDead == true)
                {
                    _currentWave++;
                    Wave2Finished?.Invoke();
                    Debug.Log("Finished 2");
                }
                break;

            case Wave.Three:
                if (_boss.Fighter.IsDead == true)
                {
                    Wave3Finished?.Invoke();
                    Debug.Log("Finished 3");
                }
                break;
        }
    }

    private void RemoveUnit(List<UnitStep> units, UnitStep unitStep)
    {
        if (units.Contains(unitStep))
        {
            units.Remove(unitStep);
            unitStep.Fighter.Died -= RemoveDiedUnit;
        }
    }

    private void RemoveUnit(UnitStep unitStep)
    {
        unitStep.Fighter.Died -= RemoveDiedUnit;
    }

    private void RegisterDie(List<UnitStep> units)
    {
        foreach (var mob in units)
            mob.Fighter.Died += RemoveDiedUnit;
    }

    private void RegisterDie(UnitStep unitStep)
    {
        unitStep.Fighter.Died += RemoveDiedUnit;
    }
}

public enum Wave
{
    One, Two, Three
}
