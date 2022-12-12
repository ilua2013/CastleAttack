using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimator : MonoBehaviour
{
    private Animator _animator;
    private UnitFriend _unitFriend;

    private SpellSpawner[] _spellSpawners;

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
    }

    private void OnEnable()
    {
        foreach (SpellSpawner spawner in _spellSpawners)
            spawner.Cast_get += OnSpellCast;

        _unitFriend.Fighter.Damaged += OnDamaged;
        _unitFriend.Fighter.Died += OnDie;
        _unitFriend.Fighter.Attacked_get += OnAttacked;
    }

    private void OnDisable()
    {
        foreach (SpellSpawner spawner in _spellSpawners)
            spawner.Cast_get -= OnSpellCast;

        _unitFriend.Fighter.Damaged -= OnDamaged;
        _unitFriend.Fighter.Died -= OnDie;
        _unitFriend.Fighter.Attacked_get -= OnAttacked;
    }

    private void OnSpellCast(/*Vector3 place, Spell spell*/)
    {
        _animator.Play(State.SpellCast.ToString());
    }

    private void OnDamaged(int damage)
    {
        _animator.Play(State.Damage.ToString());
    }

    private void OnAttacked(Transform target)
    {
        _animator.Play(State.Attack.ToString());
    }

    private void OnDie()
    {
        _animator.Play(State.Death.ToString());
    }
}
