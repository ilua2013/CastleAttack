using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimator : MonoBehaviour
{
    [SerializeField] private Arrow _arrow;
    [SerializeField] private Transform _stick;

    private Animator _animator;
    private UnitFriend _unitFriend;

    private SpellSpawner[] _spellSpawners;
    private UnitSpawner[] _unitSpawners;

    enum State
    {
        SpawnCast,
        SpellCast,
        Damage,
        Death,
        Attack,
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _unitFriend = GetComponent<UnitFriend>();

        _spellSpawners = FindObjectsOfType<SpellSpawner>();
        _unitSpawners = FindObjectsOfType<UnitSpawner>();
    }

    private void OnEnable()
    {
        foreach (SpellSpawner spawner in _spellSpawners)
            spawner.Cast += OnSpellCast;

        foreach (UnitSpawner spawner in _unitSpawners)
            spawner.SpawnedUnit += OnSpawn;

        _unitFriend.Fighter.Damaged += OnDamaged;
        _unitFriend.Fighter.Died += OnDie;
        _unitFriend.Fighter.Attacked_get += OnAttacked;
    }

    private void OnDisable()
    {
        foreach (SpellSpawner spawner in _spellSpawners)
            spawner.Cast -= OnSpellCast;

        foreach (UnitSpawner spawner in _unitSpawners)
            spawner.SpawnedUnit -= OnSpawn;

        _unitFriend.Fighter.Damaged -= OnDamaged;
        _unitFriend.Fighter.Died -= OnDie;
        _unitFriend.Fighter.Attacked_get -= OnAttacked;
    }

    private void OnSpellCast(Vector3 place, Spell spell)
    {
        _animator.Play(State.SpellCast.ToString());
    }

    private void OnSpawn(IUnit unit)
    {
        _animator.Play(State.SpawnCast.ToString());
    }

    private void OnDamaged(int damage)
    {
        _animator.Play(State.Damage.ToString());
    }

    private void OnAttacked(Transform target)
    {
        Arrow projectile = Instantiate(_arrow, _stick.position, Quaternion.identity);
        projectile.FlyTo(target.position);

        _animator.Play(State.Attack.ToString());
    }

    private void OnDie()
    {
        _animator.Play(State.Death.ToString());
    }
}
