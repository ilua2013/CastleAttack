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

    private MoverOnCell _mover;
    private Fighter _fighter;
    private Card _card;
    private int _currentStep;
    private bool _canReturnToHand = false;

    public Fighter Fighter => _fighter;
    public MoverOnCell Mover => _mover;
    public Card Card => _card;
    public TeamUnit TeamUnit => _team;
    public int CurrentStep => _currentStep;
    public bool CanReturnToHand => _canReturnToHand;

    public event Action Returned;
    public event Action Attacked;
    public event Action Moved;
    public event Action Inited;
    public event Action EndedSteps;

    private void Awake()
    {
        _mover = GetComponent<MoverOnCell>();
        _fighter = GetComponent<Fighter>();
        _currentStep = _maxStep;
        Inited?.Invoke();
    }

    private void OnEnable()
    {
        _fighter.Died += OnDie;
        _mover.ReachedLastCell += TryReturnToHand;
    }

    private void OnDisable()
    {
        _fighter.Died -= OnDie;
        _mover.ReachedLastCell -= TryReturnToHand;
    }

    public void Init(Card card, Cell currentCell, TeamUnit teamUnit)
    {
        _card = card;
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
        if (_currentStep <= 0)
            return;

        if (_fighter.TryAttack(_team))
        {
            Attacked?.Invoke();
        }
        else if(_mover.CanMove(_team))
        {
            _mover.Move(_team);            
            Moved?.Invoke();
        }
        else
        {
            _currentStep++;
        }

        _currentStep--;

        if (_currentStep <= 0)
            EndedSteps?.Invoke();
    }

    public void UpdateStep()
    {
        _currentStep = _maxStep;
    }

    public void TryReturnToHand()
    {
        //if (_canReturnToHand == false)
        //    return;

        _mover.Die();
        _fighter.Die();

        ReturnToHand();
        Card.ComeBack();
    }

    private void OnReachedLastCell()
    {
        _canReturnToHand = true;
    }

    private void OnDie(Fighter fighter = null)
    {
        _mover.Die();
    }
}

public enum TeamUnit
{
    Enemy, Friend
}
