using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private Button _buttonStartFight;
    [SerializeField] private CardsMover _cardMover;
    [SerializeField] private float _delayBeetwenStep;

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
        _cardMover = FindObjectOfType<CardsMover>();
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

    private void AddUnit(UnitFriend unitFriend)
    {
        if (_unitFriend.Contains(unitFriend) == false)
        {
            _unitFriend.Add(unitFriend);
            CalculateFirstStep();
        }
    }

    private void StartUnitsDoStep()
    {
        if (_unitFriend.Count > 0)
        {
            StartCoroutine(Battle());
            StepStarted?.Invoke();
        }
    }

    private IEnumerator Battle()
    {
        while (CheckHaveStep())
        {
            for (int i = _unitFriend.Count - 1; i > -1; i--) // определяет верный порядок действий
                _unitFriend[i].DoStep();

            yield return new WaitForSeconds(_delayBeetwenStep);

            for (int i = _unitEnemy.Count - 1; i > -1; i--)
                _unitEnemy[i].DoStep();

            yield return new WaitForSeconds(_delayBeetwenStep);
        }

        StepFinished?.Invoke();

        foreach (var item in _unitFriend)
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
}
