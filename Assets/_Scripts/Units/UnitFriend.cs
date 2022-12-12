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
    public event Action RotatedToWizzard;
    public event Action RotatedToBattle;
    public event Action<IUnit> EnemyKilled;
    public event Action<UnitFriend> LevelUpped; 
    public event Action<bool> UnitSteped;
    public event Action StartedWalking;

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
    }

    private void OnDisable()
    {
        Fighter.Died -= OnDie;
        Mover.ReachedHigherCell -= ReturnToHand;

        if(_phaseSwitcher != null)
        _phaseSwitcher.UnRegister(this);
    }

    public void LevelUp(UnitFriend unit)
    {
        LevelUpped?.Invoke(unit);
    }

    public void UnitsStartedWalking()
    {
        StartedWalking?.Invoke();
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

        if (Fighter.FighterType != FighterType.MainWizzard)
        {
            transform.forward = new Vector3(0,0,-1);
            RotatedToWizzard?.Invoke();
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

    public void DoStep(IUnit enemy = null)
    {
        if (CurrentStep <= 0)
            return;

        if (enemy == null)
            enemy = TryAttack();

        _doingStep = true;
        UnitSteped?.Invoke(_doingStep);

        if (enemy != null)
        {
            if (Fighter.Attack(enemy.Fighter, () => StartCoroutine(FinishStep(FinishedStep, 0.2f))))
                EnemyKilled?.Invoke(enemy);

            Attacked?.Invoke();

            CurrentStep--;
        }
        else if (Mover.CanMove(Mover.CurrentCell.Top))
        {
            Mover.Move(Mover.CurrentCell.Top, () => StartCoroutine(FinishStep(FinishedStep, 0.2f)));

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

    public IEnumerator FinishStep(Action action, float time)
    {
        yield return new WaitForSeconds(time);
        _doingStep = false;
        UnitSteped?.Invoke(_doingStep);
        action?.Invoke();
    }

    public void RotateTo(Transform transform, Action onRotated = null, Action onEnd = null)
    {
        print("RotateFighter");
        if (_coroutineRotateTo != null)
        {
            StopCoroutine(_coroutineRotateTo);
            _coroutineRotateTo = null;
        }

        _coroutineRotateTo = Fighter.RotateTo(transform, () => { _coroutineRotateTo = null; onEnd?.Invoke(); }, onRotated);
        StartCoroutine(_coroutineRotateTo);
    }

    public void StartMove(Cell cell, Action onEnd = null) => StartCoroutine(Mover.MoveTo(cell, onEnd));
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
                if (_coroutineRotateTo != null)
                {
                    StopCoroutine(_coroutineRotateTo);
                    _coroutineRotateTo = null;
                }
                RotatedToWizzard?.Invoke();
                _coroutineRotateTo = Mover.RotateTo(new Vector3(0,180,0), () => _coroutineRotateTo = null);
                StartCoroutine(_coroutineRotateTo);
            }
            else if (phase.IsActive && phase.PhaseType == PhaseType.Battle)
            {
                if (_coroutineRotateTo != null)
                {
                    StopCoroutine(_coroutineRotateTo);
                    _coroutineRotateTo = null;
                }
                RotatedToBattle?.Invoke();
                _coroutineRotateTo = Mover.RotateTo(new Vector3(0,0,0), () => _coroutineRotateTo = null);
                StartCoroutine(_coroutineRotateTo);
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