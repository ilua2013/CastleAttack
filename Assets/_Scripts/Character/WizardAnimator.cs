using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimator : MonoBehaviour
{
    [SerializeField] private SpellsRecorder _spellsRecorder;
    [SerializeField] private CardsSelection _cardSelection;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private Animator _animator;

    private UnitFriend _unitFriend;

    enum State
    {
        SpawnCast,
        SpellCast,
        Hit,
        Death,
        Attack,
        Battle,
        Win,
        CardsTake,
    }

    private void OnValidate()
    {
        if (_spellsRecorder == null)
            _spellsRecorder = FindObjectOfType<SpellsRecorder>();

        if (_cardSelection == null)
            _cardSelection = FindObjectOfType<CardsSelection>();

        if (_battleSystem == null)
            _battleSystem = FindObjectOfType<BattleSystem>();
    }

    private void Awake()
    {
        _unitFriend = GetComponent<UnitFriend>();
    }

    private void Start()
    {
        foreach (SpellSpawner spawner in _spellsRecorder.SpellSpawners)
            spawner.Cast_get += OnSpellCast;

        _unitFriend.Fighter.Damaged += OnDamaged;
        _unitFriend.Fighter.Died += OnDie;
        _unitFriend.Fighter.Attacked_get += OnAttacked;
        _cardSelection.CardSelected += OnCardSelected;
        _battleSystem.BattleStarted += OnBattleStarted;
    }

    private void OnDisable()
    {
        foreach (SpellSpawner spawner in _spellsRecorder.SpellSpawners)
            spawner.Cast_get -= OnSpellCast;

        _unitFriend.Fighter.Damaged -= OnDamaged;
        _unitFriend.Fighter.Died -= OnDie;
        _unitFriend.Fighter.Attacked_get -= OnAttacked;
        _cardSelection.CardSelected -= OnCardSelected;
        _battleSystem.BattleStarted -= OnBattleStarted;
    }

    private void OnSpellCast(Vector3 place, Spell spell)
    {
        _animator.Play(State.SpellCast.ToString());
    }

    private void OnDamaged(int damage)
    {
        _animator.Play(State.Hit.ToString());
    }

    private void OnAttacked(Transform target)
    {
        _animator.Play(State.Attack.ToString());
    }

    private void OnDie()
    {
        _animator.Play(State.Death.ToString());
    }

    private void OnCardSelected()
    {
        _animator.Play(State.CardsTake.ToString());
    }

    private void OnBattleStarted()
    {
        _animator.Play(State.Battle.ToString());
    }

    private void OnWin()
    {
        _animator.Play(State.Win.ToString());
    }
}
