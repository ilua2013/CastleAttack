using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFriend : MonoBehaviour, IUnit
{
    [SerializeField] private int _maxStep = 3;
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
        Card.ComeBack();

        Returned?.Invoke();
        Destroy(gameObject);
    }

    public void DoStep()
    {
        if (_currentStep <= 0)
            return;

        UnitEnemy enemy = TryAttack();

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
        List<IUnit> units = new List<IUnit>();

        var unitsTop = Mover.CurrentCell.GetUnits(Mover.CurrentCell.GetTopCell(_topDistance));       
        units.AddRange(unitsTop);

        var unitsRight = Mover.CurrentCell.GetUnits(Mover.CurrentCell.GetRightCell(_rightDistance));
        units.AddRange(unitsRight);

        var unitsLeft = Mover.CurrentCell.GetUnits(Mover.CurrentCell.GetLeftCell(_leftDistance));
        units.AddRange(unitsLeft);

        var unitsBottom = Mover.CurrentCell.GetUnits(Mover.CurrentCell.GetBottomCell(_bottomDistance));
        units.AddRange(unitsBottom);


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
