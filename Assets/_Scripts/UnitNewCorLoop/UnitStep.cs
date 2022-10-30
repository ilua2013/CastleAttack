using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MoverOnCell))]
[RequireComponent(typeof(Fighter))]
public class UnitStep : MonoBehaviour
{
    [SerializeField] private int _maxStep = 3;

    private MoverOnCell _moverOnCell;
    private Fighter _fighter;
    private Card _card;
    private int _currentStep;

    public Fighter Fighter => _fighter;
    public MoverOnCell Mover => _moverOnCell;
    public Card Card => _card;
    public int CurrentStep => _currentStep;

    public event Action Returned;
    public event Action Attacked;
    public event Action Moved;

    private void Awake()
    {
        _moverOnCell = GetComponent<MoverOnCell>();
        _fighter = GetComponent<Fighter>();
        _currentStep = _maxStep;
    }

    public void Init(Card card, Cell currentCell)
    {
        _card = card;
        _moverOnCell.SetCurrentCell(currentCell);
    }

    public void ReturnToHand()
    {
        Returned?.Invoke();
        Destroy(gameObject);
    }

    public void DoStep()
    {
        if (_fighter.TryAttack())
        {            
            Attacked?.Invoke();
            return;
        }
        else
        {
            Moved?.Invoke();
            _moverOnCell.MoveForward();            
        }

        _currentStep--;
    }

    public void UpdateStep()
    {
        _currentStep = _maxStep;
    }
}
