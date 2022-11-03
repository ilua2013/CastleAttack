using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewUnitFriend : MonoBehaviour, NewIUnit
{
    [SerializeField] private int _maxStep = 3;
    [field:SerializeField] public NewMover Mover { get; private set; }
    [field:SerializeField] public NewFighter Fighter { get; private set; }

    public Card Card { get; private set; }
    private int _currentStep;

    public int CurrentStep => _currentStep;

    public event Action Returned;
    public event Action Attacked;
    public event Action Moved;
    public event Action Inited;
    public event Action EndedSteps;

    private void Awake()
    {
        _currentStep = _maxStep;
    }

    private void OnEnable()
    {
        Fighter.Died += OnDie;
        Mover.ReachedHigherCell += ReturnToHand;
        Mover.CellChanged += StartMove;
    }

    private void OnDisable()
    {
        Fighter.Died -= OnDie;
        Mover.ReachedHigherCell -= ReturnToHand;
        Mover.CellChanged -= StartMove;
    }

    public void Init(Card card, Cell currentCell)
    {
        Card = card;
        Mover.Init(this, transform, currentCell);
        Fighter.Init();

        Inited?.Invoke();
    }

    public void ReturnToHand()
    {
        Mover.Die();
        Fighter.Die();
        Card.ComeBack();

        Returned?.Invoke();
        Destroy(gameObject);
    }

    public void DoStep()
    {
        if (_currentStep <= 0)
            return;

        NewUnitEnemy enemy = TryAttack();

        if(enemy != null)
        {
            Fighter.Attack(enemy.Fighter);
            Attacked?.Invoke();
        }
        else if (Mover.CanMove(Mover.CurrentCell.Top))
        {
            Mover.Move(Mover.CurrentCell.Top);
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

    private NewUnitEnemy TryAttack()
    {
        var units = Mover.CurrentCell.GetUnits(Mover.CurrentCell.GetForwardsCell(Fighter.DistanceAttack));

        foreach (var item in units)
            if (item is NewUnitEnemy unitEnemy)
                return unitEnemy;

        return null;
    }

    private void OnDie()
    {
        Mover.Die();
    }

    private void StartMove(Cell cell) => StartCoroutine(Mover.MoveTo(cell));
}
