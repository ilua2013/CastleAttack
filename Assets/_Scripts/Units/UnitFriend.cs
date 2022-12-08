using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitFriend : MonoBehaviour, IUnit, IRadiusAttack, IPhaseHandler
{
    [field: SerializeField] private DistanceAttack[] _distanceAttack;
    [field:SerializeField] public Mover Mover { get; private set; }
    [field:SerializeField] public Fighter Fighter { get; private set; }
    [Header("Rotate to wizzard")]
    [SerializeField] private UnitFriend _wizzard;
    [SerializeField] private PhaseSwitcher _phaseSwitcher;
    [SerializeField] private Phase[] _phases;

    public int MaxStep { get; private set; } = 1; 
    public UnitCard Card { get; private set; }
    public int CurrentStep { get; private set; }
    public bool Initialized { get; private set; }

    private IEnumerator _coroutineRotateTo;
    private IEnumerator _coroutineRotateToWizzard;
    private bool _isTutorialUnitStop = true;
    private bool _doingStep;

    public bool DoingStep => _doingStep;
    public DistanceAttack[] DistanceAttack => _distanceAttack;

    public Phase[] Phases => throw new NotImplementedException();

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

        if(Fighter.FighterType == FighterType.MainWizzard)
        {
            foreach (var item in FindObjectsOfType<Cell>())
            {
                if (item.CellIs == CellIs.Wizzard)
                    Mover.SetStartCell(item);
            }
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

        if(_phaseSwitcher != null)
        _phaseSwitcher.UnRegister(this);
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

        _phaseSwitcher = FindObjectOfType<PhaseSwitcher>();
        _phaseSwitcher.Register(this);

        foreach (var item in FindObjectsOfType<UnitFriend>())
        {
            if (item.Fighter.FighterType == FighterType.MainWizzard)
                _wizzard = item;
        }

        if (Fighter.FighterType != FighterType.MainWizzard)
        {
            Vector3 target = _wizzard.transform.position - transform.position;
            target.y = 0;
            transform.forward = target;
        }


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

        _phaseSwitcher = FindObjectOfType<PhaseSwitcher>();
        _phaseSwitcher.Register(this);

        foreach (var item in FindObjectsOfType<UnitFriend>())
        {
            if (item.Fighter.FighterType == FighterType.MainWizzard)
                _wizzard = item;
        }

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
            StartCoroutine(FinishStep(FinishedStep, 0.7f));

            CurrentStep--;
        }
        else if (Mover.CanMove(Mover.CurrentCell.Top))
        {
            Mover.Move(Mover.CurrentCell.Top);

            Moved?.Invoke();
            StartCoroutine(FinishStep(FinishedStep, 0.6f));

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
        if (_coroutineRotateTo != null)
        {
            StopCoroutine(_coroutineRotateTo);
            _coroutineRotateTo = null;
        }

        _coroutineRotateTo = Fighter.RotateTo(transform, () => _coroutineRotateTo = null, onRotated);
        StartCoroutine(_coroutineRotateTo);
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

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        if (Fighter.FighterType != FighterType.MainWizzard)
        {
            Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

            yield return new WaitForSeconds(phase.Delay);

            if (phase.IsActive && phase.PhaseType == PhaseType.SelectionCard)
            {
                if (_coroutineRotateToWizzard != null)
                {
                    StopCoroutine(_coroutineRotateToWizzard);
                    _coroutineRotateToWizzard = null;
                }

                _coroutineRotateToWizzard = Mover.RotateTo(_wizzard.transform.position, () => _coroutineRotateToWizzard = null);
                StartCoroutine(_coroutineRotateToWizzard);
            }
            else if (phase.IsActive && phase.PhaseType == PhaseType.Battle)
            {
                if (_coroutineRotateToWizzard != null)
                {
                    StopCoroutine(_coroutineRotateToWizzard);
                    _coroutineRotateToWizzard = null;
                }

                _coroutineRotateToWizzard = Mover.RotateTo(transform.position + new Vector3(0, 0, 5), () => _coroutineRotateToWizzard = null);
                StartCoroutine(_coroutineRotateToWizzard);
            }
        }
    }

    public Arrow SpawnArrow(Arrow arrow, Vector3 position)
    {
        return Instantiate(arrow, position, Quaternion.identity);
    }
}

[Serializable]
public class DistanceAttack
{
    [SerializeField] public CellNeighbor Side;
    [SerializeField] public int Distance;
}