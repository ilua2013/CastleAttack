using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MoverOnCell))]
[RequireComponent(typeof(Fighter))]
public class UnitStep : MonoBehaviour
{
    [SerializeField] private TeamUnit _team;
    [SerializeField] private int _maxStep = 3;
    [SerializeField] private HealthBar _healthBar;

    private MoverOnCell _mover;
    private Fighter _fighter;
    private Card _card;
    private int _currentStep;
    

    public Fighter Fighter => _fighter;
    public MoverOnCell Mover => _mover;
    public Card Card => _card;
    public TeamUnit TeamUnit => _team;
    public int CurrentStep => _currentStep;

    public event Action Returned;
    public event Action Attacked;
    public event Action Moved;
    public event Action Inited;


    private void Awake()
    {
        _mover = GetComponent<MoverOnCell>();
        _fighter = GetComponent<Fighter>();
        _currentStep = _maxStep;
       
        Inited?.Invoke();
    }

    public void Init(Card card, Cell currentCell, TeamUnit teamUnit)
    {
        _healthBar.Init(_fighter);
        _card = card;
        _mover.SetCurrentCell(currentCell);
        _team = teamUnit;
    }

    public void EnemyInit(Cell currentCell, TeamUnit teamUnit)
    {
        _mover.SetCurrentCell(currentCell);
        _team = teamUnit;
    }

    public void ReturnToHand()
    {
        Returned?.Invoke();
        Destroy(gameObject);
    }

    public void DoStep()
    {
        if (_fighter.TryAttack(_team))
        {
            Attacked?.Invoke();
            return;
        }
        else
        {
            Moved?.Invoke();
            _mover.Move(_team);
        }

        _currentStep--;
    }

    public void UpdateStep()
    {
        _currentStep = _maxStep;
    }
}

public enum TeamUnit
{
    Enemy, Friend
}
