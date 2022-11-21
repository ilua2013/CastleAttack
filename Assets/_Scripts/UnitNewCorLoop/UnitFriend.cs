using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFriend : MonoBehaviour, IUnit, IRadiusAttack
{
    [SerializeField] private int _maxStep = 3;
    [field: SerializeField] private DistanceAttack[] _distanceAttack;
    [field:SerializeField] public Mover Mover { get; private set; }
    [field:SerializeField] public Fighter Fighter { get; private set; }   

    public UnitCard Card { get; private set; }
    private int _currentStep;
    private bool _isTutorialUnitStop = true;

    public int CurrentStep => _currentStep;
    public bool Initialized { get; private set; }

    public event Action Returned;
    public event Action Attacked;
    public event Action Moved;
    public event Action Inited;
    public event Action EndedSteps;
    public event Action StopUnit;

    private void OnValidate()
    {
        RaycastHit raycastHit;
        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out raycastHit, 50, 1, QueryTriggerInteraction.Collide);

        if(raycastHit.collider != null && raycastHit.collider.TryGetComponent(out Cell cell))
        {
            Mover.SetStartCell(cell);
            transform.position = cell.transform.position;
        }
    }

    private void Awake()
    {
        _currentStep = _maxStep;

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
        Initialized = true;
    }

    public void ReturnToHand()
    {
        StartCoroutine(TutorialPause());
        
    }

    public IEnumerator TutorialPause()
    {

        yield return new WaitForSeconds(0.1f);
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

    public List<Cell> RadiusView()
    {
        List<Cell> cells = Mover.CurrentCell.GetCellsDistanceAttack(_distanceAttack);
        return cells;
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