using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFriend : MonoBehaviour, IUnit, IRadiusAttack
{
    [field: SerializeField] public int MaxStep { get; private set; } = 3; 
    [field: SerializeField] private DistanceAttack[] _distanceAttack;
    [field:SerializeField] public Mover Mover { get; private set; }
    [field:SerializeField] public Fighter Fighter { get; private set; }   

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
    public event Action StopUnit;
    public event Action FinishedStep;
    public event Action StepChanged;
    public event Action<UnitEnemy> EnemyKilled;
    public event Action<UnitFriend> LevelUpped;

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
        CurrentStep = MaxStep;

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

    public void LevelUp(UnitFriend unit)
    {
        LevelUpped?.Invoke(unit);
    }

    public void ReturnToHand()
    {
        StartCoroutine(DestroyWithDelay(0f, Card.ComeBack));
    }

    public void Init(UnitCard card, Cell currentCell)
    {
        Card = card;

        Mover.Init(this, transform, currentCell);

        if (Card != null)
            Fighter.Init(this, transform, GetDamage(Card), GetHealth(Card));
        else
            Fighter.Init(this, transform);

        Inited?.Invoke();
        Initialized = true;
    }

    public void Init(UnitCard card, Cell currentCell, int currentStep)
    {
        Card = card;
        CurrentStep = currentStep;

        Mover.Init(this, transform, currentCell);

        if (Card != null)
            Fighter.Init(this, transform, GetDamage(Card), GetHealth(Card));
        else
            Fighter.Init(this, transform);

        Inited?.Invoke();
        Initialized = true;
    }

    public IEnumerator DestroyWithDelay(float delay, Action onEnd = null)
    {
        yield return new WaitForSeconds(delay);

        Mover.Die();
        Fighter.Die();        
        gameObject.SetActive(false);
        onEnd?.Invoke();
    }

    public void DoStep()
    {
        if (CurrentStep <= 0)
            return;

        UnitEnemy enemy = TryAttack();
        _doingStep = true;

        if (enemy != null)
        {
            if (Fighter.Attack(enemy.Fighter))
                EnemyKilled?.Invoke(enemy);

            Attacked?.Invoke();
            StartCoroutine(FinishStep(FinishedStep, 0.5f));

            CurrentStep--;
        }
        else if (Mover.CanMove(Mover.CurrentCell.Top))
        {
            Mover.Move(Mover.CurrentCell.Top);

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

    private UnitEnemy TryAttack()
    {
        List<UnitEnemy> units = Mover.CurrentCell.GetEnemyUnits(_distanceAttack);

        foreach (var item in units)
                return item;

        return null;
    }

    public bool CanAttackDiagonal()
    {
        for (int i = 0; i < _distanceAttack.Length; i++)
        {
            CellNeighbor side = _distanceAttack[i].Side;

            if (side == CellNeighbor.BotLeft || side == CellNeighbor.BotRight || side == CellNeighbor.TopRight || side == CellNeighbor.TopLeft)
                return true;
        }

        return false;
    }

    public List<Cell> RadiusView()
    {
        List<Cell> cells = Mover.CurrentCell.GetCellsDistanceAttack(_distanceAttack);
        return cells;
    }

    private void OnDie()
    {
        Mover.Die();

        if (Fighter.FighterType == FighterType.MainWizzard)
        {
            GamesStatistics.RegisterFriendKill();
            gameObject.SetActive(false);
        }      
    }

    private IEnumerator FinishStep(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        _doingStep = false;
        action?.Invoke();
    }

    public void RotateTo(Transform transform, Action onRotated = null)
    {
        if (_coroutineRotateTo == null)
        {
            _coroutineRotateTo = StartCoroutine(Fighter.RotateTo(transform, () => _coroutineRotateTo = null, onRotated));
        }
        else
        {
            StopCoroutine(_coroutineRotateTo);
            _coroutineRotateTo = StartCoroutine(Fighter.RotateTo(transform, () => _coroutineRotateTo = null, onRotated));
        }
    }

    private void StartMove(Cell cell) => StartCoroutine(Mover.MoveTo(cell));
    public void AnimationSizeUp() => StartCoroutine(Mover.AnimationSizeUp());

    private int GetDamage(UnitCard card)
    {
        int damage = card.CardSave.UnitStats.Damage;

        if (card.Stage == CardStage.Two)
            damage += 2;

        if (card.Stage == CardStage.Three)
            damage += 4;

        return damage;
    }

    private int GetHealth(UnitCard card)
    {
        int health = card.CardSave.UnitStats.MaxHealth;

        if (card.Stage == CardStage.Two)
            health += 2;

        if (card.Stage == CardStage.Three)
            health += 4;

        return health;
    }
}

[Serializable]
public class DistanceAttack
{
    [SerializeField] public CellNeighbor Side;
    [SerializeField] public int Distance;
}