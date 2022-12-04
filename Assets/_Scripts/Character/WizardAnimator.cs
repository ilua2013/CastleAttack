using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimator : MonoBehaviour
{
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
    }

    private void OnDisable()
    {
        foreach (SpellSpawner spawner in _spellSpawners)
            spawner.Cast -= OnSpellCast;

        foreach (UnitSpawner spawner in _unitSpawners)
            spawner.SpawnedUnit -= OnSpawn;

        _unitFriend.Fighter.Damaged -= OnDamaged;
        _unitFriend.Fighter.Died -= OnDie;
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

    private void OnDie()
    {
        _animator.Play(State.Death.ToString());
    }
}
