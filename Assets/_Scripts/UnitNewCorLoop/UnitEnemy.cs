using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitEnemy : MonoBehaviour, IUnit, IRadiusAttack
{
    [SerializeField] private int _maxStep = 3;
    [field: SerializeField] private DistanceAttack[] _distanceAttack;
    [field: SerializeField] public Mover Mover { get; private set; }
    [field: SerializeField] public Fighter Fighter { get; private set; }

    public UnitCard Card { get; private set; }
    private int _currentStep;
    private bool _doingStep;

    public int CurrentStep => _currentStep;
    public bool Initialized { get; private set; }
    public bool DoingStep => _doingStep;
    public DistanceAttack[] DistanceAttack => _distanceAttack;

    public event Action Returned;
    public event Action Attacked;
    public event Action Moved;
    public event Action Inited;
    public event Action EndedSteps;
    public event Action FinishedStep;

    private void OnValidate()
    {
        RaycastHit raycastHit;
        var raycasts = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, 50, 1, QueryTriggerInteraction.Collide);

        for (int i = 0; i < raycasts.Length; i++)
        {
            if (raycasts[i].collider.TryGetComponent(out Cell cell))
            {
                Mover.SetStartCell(cell);
                transform.position = cell.transform.position;
                break;
            }
        }
    }

    private void Awake()
    {
        _currentStep = _maxStep;

        if (Fighter.FighterType == FighterType.MainTarget)
            Init(null, null);
    }

    private void Start()
    {
        if (Initialized == false)
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

    public void Init(UnitCard card, Cell cell)
    {
        Card = card;
        Mover.Init(this, transform, cell);
        Fighter.Init(this, transform);

        Inited?.Invoke();
        Initialized = true;
    }

    public void DoStep()
    {
        if (_currentStep <= 0)
            return;

        UnitFriend enemy = TryAttack();
        _doingStep = true;

        if (enemy != null)
        {
            Fighter.Attack(enemy.Fighter);

            Attacked?.Invoke();
            StartCoroutine(FinishStep(FinishedStep, 0.5f));

            _currentStep -= 2;
        }
        else if (Mover.CanMove(Mover.CurrentCell.Bot))
        {
            Mover.Move(Mover.CurrentCell.Bot);

            Moved?.Invoke();
            StartCoroutine(FinishStep(FinishedStep, 0.7f));

            _currentStep--;
        }
        else
        {
            _currentStep--;
            StartCoroutine(FinishStep(FinishedStep, 0));
        }

        if (_currentStep <= 0)
            EndedSteps?.Invoke();
    }

    public void UpdateStep()
    {
        _currentStep = _maxStep;
    }

    private UnitFriend TryAttack()
    {
        List<UnitFriend> units = Mover.CurrentCell.GetFriendUnits(_distanceAttack);

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

        if(Fighter.FighterType == FighterType.Build|| Fighter.FighterType == FighterType.MainTarget)
        {
            GamesStatistics.RegisterEnemyKill();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator FinishStep(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        _doingStep = false;
        action?.Invoke();
    }

    private void StartMove(Cell cell) => StartCoroutine(Mover.MoveTo(cell));

    public void RotateTo(Transform transform) => StartCoroutine(Fighter.RotateTo(transform));
}
