using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class FightSystem : MonoBehaviour
{
    [SerializeField] private Button _buttonStartFight;
    [SerializeField] private float _delayBeetwenStep;
    [SerializeField] private UnitSpawner[] _spawners;

    private List<UnitFriend> _unitFriend = new List<UnitFriend>();
    private List<UnitEnemy> _unitEnemy = new List<UnitEnemy>();
    private bool _doStep = true;

    public int CountEnemy => _unitEnemy.Count;
    public List<UnitEnemy> UnitsEnemy => _unitEnemy;

    public event Action StepFinished;
    public event Action StepStarted;
    public event Action RemovedDie;

    private void OnValidate()
    {
       // _spawners = FindObjectsOfType<UnitSpawner>();
    }

    private void OnEnable()
    {
        _buttonStartFight.onClick.AddListener(StartUnitsDoStep);
    }

    private void OnDisable()
    {
        _buttonStartFight.onClick.RemoveListener(StartUnitsDoStep);
    }

    public void StopDoStep()
    {
        _doStep = false;
    }

    private void StartUnitsDoStep()
    {
        if (_unitFriend.Count > 0)
        {
            StepStarted?.Invoke();
        }
    }

    private void DoStep(List<IUnit> unitSteps)
    {
        //for (int i = unitSteps.Count - 1; i > -1; i--)
        //    unitSteps[i].DoStep();
    }
}
