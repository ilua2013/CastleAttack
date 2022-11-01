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
    [SerializeField] private UnitStep[] _enemyInScene;
    [SerializeField] private UnitStep[] _wallFriedlyInScene;

    private List<UnitStep> _unitFriend = new List<UnitStep>();
    private List<UnitStep> _unitEnemy = new List<UnitStep>();

    public event Action StepFinished;
    public event Action StepStarted;

    private void OnValidate()
    {
        _spawners = FindObjectsOfType<UnitSpawner>();

        var enemys = FindObjectsOfType<UnitStep>();
        List<UnitStep> listEnemy = new List<UnitStep>();

        foreach (var item in enemys)
            if(item.TeamUnit == TeamUnit.Enemy)
                listEnemy.Add(item);

        _enemyInScene = listEnemy.ToArray();
    }

    private void Awake()
    {
        foreach (var item in _enemyInScene)
            AddUnit(item);
        foreach (var item in _wallFriedlyInScene)
            AddUnit(item);

    }

    private void OnEnable()
    {
        _buttonStartFight.onClick.AddListener(StartUnitsDoStep);

        foreach (var spawner in _spawners)
            spawner.SpawnedUnit += AddUnit;
    }

    private void OnDisable()
    {
        _buttonStartFight.onClick.RemoveListener(StartUnitsDoStep);

        foreach (var spawner in _spawners)
            spawner.SpawnedUnit -= AddUnit;
    }

    private void StartUnitsDoStep()
    {
        if (_unitFriend.Count > 0)
        {
            StartCoroutine(UnitsDoStep());
            StepStarted?.Invoke();
        }
    }

    private IEnumerator UnitsDoStep()
    {
        while (CheckHaveStep())
        {
            foreach (var item in _unitFriend)
                if (item.TeamUnit == TeamUnit.Friend)
                    item.DoStep();

            yield return new WaitForSeconds(_delayBeetwenStep);

            foreach (var item in _unitEnemy)
                if (item.TeamUnit == TeamUnit.Enemy)
                    item.DoStep();

            yield return new WaitForSeconds(_delayBeetwenStep);
        }

        StepFinished?.Invoke();

        foreach (var item in _unitFriend)
            item.UpdateStep();
    }

    private void AddUnit(UnitStep unit)
    {
        if (_unitFriend.Contains(unit) == false && unit.TeamUnit == TeamUnit.Friend)
        {
            _unitFriend.Add(unit);
            unit.Fighter.Died += RemoveUnit;
        }
        else if (_unitEnemy.Contains(unit) == false && unit.TeamUnit == TeamUnit.Enemy)
        {
            _unitEnemy.Add(unit);
            unit.Fighter.Died += RemoveUnit;
        }
    }

    private void RemoveUnit(Fighter fighter)
    {
        fighter.TryGetComponent(out UnitStep unitStep);

        if (_unitFriend.Contains(unitStep) == true)
        {
            unitStep.Fighter.Died -= RemoveUnit;
            _unitFriend.Remove(unitStep);
        }
        else if (_unitEnemy.Contains(unitStep) == true)
        {
            unitStep.Fighter.Died -= RemoveUnit;
            _unitEnemy.Remove(unitStep);
        }
    }

    private bool CheckHaveStep()
    {
        foreach (var item in _unitFriend)
            if (item.CurrentStep > 0)
                return true;

        foreach (var item in _unitEnemy)
            if (item.CurrentStep > 0)
                return true;

        return false;
    }
}
