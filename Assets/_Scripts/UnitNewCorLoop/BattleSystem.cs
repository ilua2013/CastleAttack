using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private Button _buttonStartFight;
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private float _delayBeetwenStep = 0.15f;

    private List<UnitFriend> _unitFriend = new List<UnitFriend>();
    private List<UnitEnemy> _unitEnemy = new List<UnitEnemy>();
    private bool _doStep = true;
    private bool _friendFinishStep;
    private bool _enemyFinishStep;

    public int CountEnemy => _unitEnemy.Count;
    public EnemySpawner EnemySpawner => _enemySpawner;
    public List<UnitEnemy> UnitsEnemy => _unitEnemy;
    public List<UnitFriend> UnitsFriend => _unitFriend;

    public event Action StepFinished;
    public event Action StepStarted;
    public event Action RemovedDie;
    public event Action DiedAllEnemy;
    public event Action EnemySpawnerChanged;
    public event Action TutorialStopedUnit;

    private void OnValidate()
    {
        _cardsHand = FindObjectOfType<CardsHand>();
    }

    private void OnEnable()
    {
        _buttonStartFight.onClick.AddListener(StartBattle);
        _cardsHand.Spawned += AddUnit;
        _enemySpawner.Spawned_get += AddUnit;

        foreach (var unit in _unitFriend)
            unit.Mover.ReachedHigherCell += TutorialStopUnit;
    }

    private void OnDisable()
    {
        _buttonStartFight.onClick.RemoveListener(StartBattle);
        _cardsHand.Spawned -= AddUnit;
        _enemySpawner.Spawned_get -= AddUnit;

        foreach (var unit in _unitFriend)
            unit.Mover.ReachedHigherCell -= TutorialStopUnit;
    }

    public void SetEnemySpawner(EnemySpawner enemySpawner)
    {
        _enemySpawner.Spawned_get -= AddUnit;
        _enemySpawner = enemySpawner;
        _enemySpawner.Spawned_get += AddUnit;

        _unitEnemy.Clear();

        EnemySpawnerChanged?.Invoke();
    }

    public void ReturnToHandFriend()
    {
        for (int i = _unitFriend.Count - 1; i > -1; i--)
            _unitFriend[i].ReturnToHandStageUp();
    }

    private void TutorialStopUnit()
    {
        TutorialStopedUnit?.Invoke();
    }

    public void StopDoStep()
    {
        _doStep = false;
    }

    private void AddUnit(UnitFriend unitFriend)
    {
        if (unitFriend != null && _unitFriend.Contains(unitFriend) == false)
        {
            _unitFriend.Add(unitFriend);
            unitFriend.Fighter.Died_get += RemoveUnit;
            unitFriend.FinishedStep += CheckFinishStepFriend;
        }
    }

    private void AddUnit(UnitEnemy unitEnemy)
    {
        if (_unitEnemy.Contains(unitEnemy) == false)
        {
            _unitEnemy.Add(unitEnemy);
            unitEnemy.Fighter.Died_get += RemoveUnit;
            unitEnemy.FinishedStep += CheckFinishStepEnemy;
        }
    }

    private void StartBattle()
    {
        CalculateFirstStep();
        _doStep = true;

        StepStarted?.Invoke();
        StartCoroutine(Battle());
    }

    private IEnumerator Battle()
    {
        while (CheckHaveStep() && _doStep == true)
        {
            _friendFinishStep = false;
            _enemyFinishStep = false;

            for (int i = _unitFriend.Count - 1; i > -1; i--) // определяет верный порядок действий
                _unitFriend[i].DoStep();

            while (_friendFinishStep == false)
            {
                yield return new WaitForSeconds(0.2f);
                CheckFinishStepFriend();
            }

            yield return new WaitForSeconds(_delayBeetwenStep);

            for (int i = _unitEnemy.Count - 1; i > -1; i--)
                _unitEnemy[i].DoStep();

            while (_enemyFinishStep == false)
            {
                yield return new WaitForSeconds(0.2f);
                CheckFinishStepEnemy();
            }
        }

        StepFinished?.Invoke();
        print("Update Step");
        foreach (var item in _unitFriend)
            item.UpdateStep();

        foreach (var item in _unitEnemy)
            item.UpdateStep();
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

    private void CheckFinishStepFriend()
    {
        foreach (var item in _unitFriend)
        {
            if (item.DoingStep == true)
                return;
        }

        _friendFinishStep = true;
    }

    private void CheckFinishStepEnemy()
    {
        foreach (var item in _unitEnemy)
            if (item.DoingStep == true)
                return;

        _enemyFinishStep = true;
    }

    private void CalculateFirstStep()
    {
        List<UnitFriend> units = new List<UnitFriend>();
        int index = 0;
        
        while (units.Count != _unitFriend.Count)
        {
            foreach (var item in _unitFriend)
                if (item.Mover.CurrentCell.Number == index)
                    units.Add(item);

            index++;
        }

        _unitFriend = units;
        foreach (var unit in _unitFriend)
        {           
            unit.Mover.ReachedHigherCell += TutorialStopUnit;
        }
        index = 20;

        List<UnitEnemy> unitsEnemy = new List<UnitEnemy>();

        while (unitsEnemy.Count != _unitEnemy.Count)
        {
            foreach (var item in _unitEnemy)
                if (item.Mover.CurrentCell.Number == index)
                    unitsEnemy.Add(item);

            index--;
        }

        _unitEnemy = unitsEnemy;
    }

    private void RemoveUnit(Fighter fighter)
    {
        fighter.Died_get -= RemoveUnit;

        if (fighter.Unit is UnitFriend unitFriend)
            _unitFriend.Remove(unitFriend);
        else if (fighter.Unit is UnitEnemy unitEnemy)
        {
            _unitEnemy.Remove(unitEnemy);

            if (_unitEnemy.Count == 0)
                DiedAllEnemy?.Invoke();
        }
    }
}
