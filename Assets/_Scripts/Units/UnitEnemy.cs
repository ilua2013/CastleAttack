using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitEnemy : MonoBehaviour, IUnit, IRadiusAttack
{
    [field: SerializeField] public float DelayToDie { get; private set; } = 2.5f;
    [field: SerializeField] private DistanceAttack[] _distanceAttack;
    [field: SerializeField] public Mover Mover { get; private set; }
    [field: SerializeField] public Fighter Fighter { get; private set; }

    public int MaxStep { get; private set; } = 1;
    public UnitCard Card { get; private set; }
    public int CurrentStep { get; private set; }
    public bool Initialized { get; private set; }

    private Coroutine _coroutineRotateTo;
    private bool _isTutorialUnitStop = true;
    private bool _doingStep;

    public bool DoingStep => _doingStep;
    public DistanceAttack[] DistanceAttack => _distanceAttack;

    public event Action Returned;
    public event Action Attacked;
    public event Action Moved;
    public event Action Inited;
    public event Action EndedSteps;
    public event Action FinishedStep;
    public event Action StepChanged;
    public event Action LevelUppedTutorial;
    public event Action<bool> UnitSteped;
    public event Action StartedWalking;

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
        //if (Fighter.FighterType == FighterType.Cavalery)
        //    MaxStep = 2;
        CurrentStep = MaxStep;
       
    }

    private void Start()
    {
        if (Initialized == false)
            Init(null, null);
    }

    private void OnEnable()
    {
        Fighter.Died += OnDie;
    }

    private void OnDisable()
    {
        Fighter.Died -= OnDie;
    }

    public void Init(UnitCard card, Cell cell)
    {
        Card = card;
        Mover.Init(this, transform, cell);
        Fighter.Init(this, transform);

        Inited?.Invoke();
        Initialized = true;
    }

    public void UnitsStartedWalking()
    {
        StartedWalking?.Invoke();
    }

    public void CavaleryStep()
    {
        CurrentStep = MaxStep;
    }

    public void DoStep()
    {
        if (CurrentStep <= 0)
            return;       

        UnitFriend enemy = TryAttack();
        _doingStep = true;
        UnitSteped?.Invoke(_doingStep);

        if (enemy != null)
        {
            Fighter.Attack(enemy.Fighter, () => StartCoroutine(FinishStep(FinishedStep, 0.2f)));

            Attacked?.Invoke();

            CurrentStep--;
        }
        else if (Mover.CanMove(Mover.CurrentCell.Bot))
        {
            Mover.Move(Mover.CurrentCell.Bot, () => StartCoroutine(FinishStep(FinishedStep, 0.2f)));

            Moved?.Invoke();
            StartCoroutine(FinishStep(FinishedStep, 0.7f));

            CurrentStep--;
        }
        else
        {
            CurrentStep--;
            StartCoroutine(FinishStep(FinishedStep, 0));
        }

        if (CurrentStep <= 0)
        {
            CurrentStep = 0;
            EndedSteps?.Invoke();
        }

        StepChanged?.Invoke();
    }

    public void UpdateStep()
    {
        CurrentStep = MaxStep;
        StepChanged?.Invoke();
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
            LevelUppedTutorial?.Invoke();
            Invoke(nameof(Disable), DelayToDie);
           
        }
    }

    public IEnumerator FinishStep(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        _doingStep = false;
        UnitSteped?.Invoke(_doingStep);
        action?.Invoke();
    }

    public void RotateTo(Transform transform, Action onRotated = null, Action onEnd = null)
    {
        if (_coroutineRotateTo == null)
        {
            _coroutineRotateTo = StartCoroutine(Fighter.RotateTo(transform, () => { _coroutineRotateTo = null; onEnd?.Invoke(); }, onRotated));
        }
        else
        {
            StopCoroutine(_coroutineRotateTo);
            _coroutineRotateTo = StartCoroutine(Fighter.RotateTo(transform, () => { _coroutineRotateTo = null; onEnd?.Invoke(); }, onRotated));
        }
    }

    public Arrow SpawnArrow(Arrow arrow, Vector3 position)
    {
        return Instantiate(arrow, position, Quaternion.identity);
    }

    private void Disable() => gameObject.SetActive(false);
    public void StartMove(Cell cell, Action onEnd = null) => StartCoroutine(Mover.MoveTo(cell, onEnd));
    public void AnimationSizeUp() => StartCoroutine(Mover.AnimationSizeUp());
}
