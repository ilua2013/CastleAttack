using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitEnemy : MonoBehaviour, IUnit, IRadiusAttack
{
    [field: SerializeField] public int MaxStep { get; private set; } = 1;
    [field: SerializeField] public float DelayToDie { get; private set; } = 2.5f;
    [field: SerializeField] private DistanceAttack[] _distanceAttack;
    [field: SerializeField] private DistanceAttack[] _distanceAttackCatapult;
    [field: SerializeField] public Mover Mover { get; private set; }
    [field: SerializeField] public Fighter Fighter { get; private set; }
    [field: SerializeField] public EnemyStats EnemyStats { get; private set; }

    public UnitCard Card { get; private set; }
    public int CurrentStep { get; private set; }
    public bool Initialized { get; private set; }
    public bool IsSkipped { get; private set; }

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

        Fighter.Init(this, transform, 
            EnemyStats.GetModifyDamage(Fighter.Damage, SaveCastle.CountDead + 1), 
            EnemyStats.GetModifyHealth(Fighter.MaxHealth, SaveCastle.CountDead + 1));

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

    public void DoStep(IUnit enemy = null)
    {
        if (IsSkipped)
        {
            IsSkipped = false;
            CurrentStep = 0;
            return;
        }

        if (CurrentStep <= 0)
            return;       

        if (enemy == null)
            enemy = TryAttack();

        _doingStep = true;

        if (enemy != null)
        {
            Fighter.Attack(enemy.Fighter, () => StartCoroutine(FinishStep(FinishedStep, 0f)));

            UnitSteped?.Invoke(_doingStep);
            Attacked?.Invoke();

            CurrentStep--;
        }
        else if (Mover.CanMove(Mover.CurrentCell.Bot))
        {
            Mover.Move(Mover.CurrentCell.Bot, () => StartCoroutine(FinishStep(FinishedStep, 0f)));

            UnitSteped?.Invoke(_doingStep);
            Moved?.Invoke();

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

    public void SkipStep()
    {
        IsSkipped = true;
    }

    public void UpdateStep()
    {
        CurrentStep = MaxStep;
        StepChanged?.Invoke();
    }

    private UnitFriend TryAttack()
    {
        List<UnitFriend> units = Mover.CurrentCell.GetFriendUnits(_distanceAttack);

        if (Fighter.FighterType == FighterType.Catapult)
        {
            UnitFriend unit = CatapultAttack();

            if (unit != null)
                units = new List<UnitFriend>() { unit };
            else
                units.Clear();
        }

        foreach (var item in units)
            return item;

        return null;
    }

    private UnitFriend CatapultAttack()
    {
        if (Mover.CurrentCell?.Bot?.Bot?.CurrentUnit is UnitFriend botUnit)
            return botUnit;

        if (Mover.CurrentCell?.Bot?.Bot?.Left?.CurrentUnit is UnitFriend leftUnit)
            return leftUnit;

        if (Mover.CurrentCell?.Bot?.Bot?.Right?.CurrentUnit is UnitFriend rightUnit)
            return rightUnit;

        return null;
    }

    public List<Cell> RadiusView()
    {
        List<Cell> cells = Mover.CurrentCell.GetCellsDistanceAttack(_distanceAttack);
        if(Fighter.FighterType == FighterType.Catapult)
        {
            List<Cell> cellsCatapult = Mover.CurrentCell.GetCellsDistanceAttackForCatapult(_distanceAttackCatapult, cells);
            cells.AddRange(cellsCatapult);
        }
           

        return cells;
    }

    private void OnDie()
    {
        Mover.Die();

        if (Fighter.FighterType == FighterType.Build|| Fighter.FighterType == FighterType.MainTarget)
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

    public Arrow SpawnArrow(Arrow arrow, Transform position)
    {
        Arrow spawned = Instantiate(arrow, position.position, Quaternion.identity);
        spawned.transform.parent = position;
        return spawned;
    }

    private void Disable() => gameObject.SetActive(false);
    public void StartMove(Cell cell, Action onEnd = null) => StartCoroutine(Mover.MoveTo(cell, onEnd));
    public void AnimationSizeUp() => StartCoroutine(Mover.AnimationSizeUp());

    public void LocalRotateTo(Transform transform, Action onRotated = null, Action onEnd = null)
    {
        if (_coroutineRotateTo == null)
        {
            _coroutineRotateTo = StartCoroutine(Fighter.LocalRotateTo(transform, () => { _coroutineRotateTo = null; onEnd?.Invoke(); }, onRotated));
        }
        else
        {
            StopCoroutine(_coroutineRotateTo);
            _coroutineRotateTo = StartCoroutine(Fighter.LocalRotateTo(transform, () => { _coroutineRotateTo = null; onEnd?.Invoke(); }, onRotated));
        }
    }
}
