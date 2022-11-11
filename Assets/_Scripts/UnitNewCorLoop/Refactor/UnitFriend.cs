using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFriend : MonoBehaviour, IUnit
{
    [SerializeField] private int _maxStep = 3;
    [field: SerializeField] private DistanceAttack[] _distanceAttack;
    [field:SerializeField] public Mover Mover { get; private set; }
    [field:SerializeField] public Fighter Fighter { get; private set; }
    [SerializeField] private int _topDistance = 0;
    [SerializeField] private int _rightDistance = 0;
    [SerializeField] private int _bottomDistance = 0;
    [SerializeField] private int _leftDistance = 0;

    public UnitCard Card { get; private set; }
    private int _currentStep;

    public int CurrentStep => _currentStep;
    public int TopDistance => _topDistance;
    public int RightDistance => _rightDistance;

    public int BottomDistance => _bottomDistance;

    public int LeftDistance => _leftDistance;

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
        if (Fighter.FighterType == FighterType.MainWizzard)
            Init(null, null);
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

    public void Init(UnitCard card, Cell currentCell)
    {
        Card = card;
        Mover.Init(this, transform, currentCell);
        Fighter.Init(this);

        Inited?.Invoke();
    }

    public void ReturnToHand()
    {
        Mover.Die();
        Fighter.Die();
        gameObject.SetActive(false);
        Card.ComeBack();
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

            _currentStep -= 2;
        }
        else if (Mover.CanMove(Mover.CurrentCell.Top))
        {
            Mover.Move(Mover.CurrentCell.Top);
            Moved?.Invoke();

            _currentStep--;
        }
        else
        {
            _currentStep--;
        }

        if (_currentStep <= 0)
            EndedSteps?.Invoke();
    }

    public void UpdateStep()
    {
        _currentStep = _maxStep;
    }

    private UnitEnemy TryAttack()
    {
        List<UnitEnemy> units = Mover.CurrentCell.GetEnemyUnits(_distanceAttack);

        foreach (var item in units)
                return item;

        return null;
    }

    private void OnDie()
    {
        Mover.Die();
        gameObject.SetActive(false);
    }

    private void StartMove(Cell cell) => StartCoroutine(Mover.MoveTo(cell));
}

[Serializable]
public class DistanceAttack
{
    [SerializeField] public CellNeighbor Side;
    [SerializeField] public int Distance;
}