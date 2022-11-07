using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitEnemy : MonoBehaviour, IUnit
{
    [SerializeField] private int _maxStep = 3;
    [field: SerializeField] public Mover Mover { get; private set; }
    [field: SerializeField] public Fighter Fighter { get; private set; }

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

    private void Start()
    {
        if (Fighter.FighterType == FighterType.MainTarget)
            Init(null, null);
    }

    private void OnEnable()
    {
        Fighter.Died += OnDie;
        Mover.CellChanged += StartMove;
    }

    private void OnDisable()
    {
        Fighter.Died -= OnDie;
        Mover.CellChanged -= StartMove;
    }

    public void Init(Card card, Cell cell)
    {
        Card = card;
        Mover.Init(this, transform, cell);
        Fighter.Init(this);

        Inited?.Invoke();
    }

    public void DoStep()
    {
        if (_currentStep <= 0)
            return;

        UnitEnemy enemy = TryAttack();

        if (enemy != null)
        {
            Fighter.Attack(enemy.Fighter);
            Attacked?.Invoke();
        }
        else if (Mover.CanMove(Mover.CurrentCell.Bot))
        {
            Mover.Move(Mover.CurrentCell.Bot);
            Moved?.Invoke();
        }

        _currentStep--;

        if (_currentStep <= 0)
            EndedSteps?.Invoke();
    }

    public void UpdateStep()
    {
        _currentStep = _maxStep;
    }

    private UnitEnemy TryAttack()
    {
        var units = Mover.CurrentCell.GetUnits(Mover.CurrentCell.GetBottomCell(Fighter.DistanceAttack));

        foreach (var item in units)
            if (item is UnitEnemy unitEnemy)
                return unitEnemy;

        return null;
    }

    private void OnDie()
    {
        Mover.Die();
        gameObject.SetActive(false);
    }

    private void StartMove(Cell cell) => StartCoroutine(Mover.MoveTo(cell));
}