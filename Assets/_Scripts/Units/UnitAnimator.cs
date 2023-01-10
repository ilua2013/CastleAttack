using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator[] _animators;

    private float _delayAnimationDamage = 0;//0.35f;
    private IUnit _unit;
    private Mover _mover;
    private Fighter _fighter;

    enum State
    {
        Attack, Move, Died, Hit, ShowRoots, HideRoots,
    }

    private void OnValidate()
    {
        _animators = GetComponentsInChildren<Animator>();
    }

    private void Awake()
    {
        _unit = GetComponent<IUnit>();
        _mover = _unit.Mover;
        _fighter = _unit.Fighter;
    }

    private void OnEnable()
    {
        _mover.StartedMove += SetMove;
        _mover.Rooted += SetRoot;
        _mover.UnRooted += SetUnRoot;
        _fighter.RotatedToAttack += SetAttack;
        _fighter.Died += StartSetDie;
        _fighter.Damaged += StartSetDamage;
    }

    private void OnDisable()
    {
        _mover.StartedMove -= SetMove;
        _mover.Rooted -= SetRoot;
        _mover.UnRooted -= SetUnRoot;
        _fighter.RotatedToAttack -= SetAttack;
        _fighter.Died -= StartSetDie;
        _fighter.Damaged -= StartSetDamage;
    }   

    private void SetAttack()
    {
        foreach (var animator in _animators)
        {
            animator.SetTrigger(State.Attack.ToString());
        }
    }

    private void StartSetDie() => Invoke(nameof(SetDie), _delayAnimationDamage);

    private void SetDie()
    {
        foreach (var animator in _animators)
        {
            animator.SetTrigger(State.Died.ToString());
        }
    }

    private void SetMove()
    {
        foreach (var animator in _animators)
        {
            animator.SetTrigger(State.Move.ToString());
        }             
    }

    private void SetRoot()
    {
        foreach (var animator in _animators)
        {
            animator.SetTrigger(State.ShowRoots.ToString());
        }
    }

    private void SetUnRoot()
    {
        foreach (var animator in _animators)
        {
            animator.SetTrigger(State.HideRoots.ToString());
        }
    }

    private void StartSetDamage(int damage) => Invoke(nameof(SetDamage), _delayAnimationDamage);

    private void SetDamage()
    {
        foreach (var animator in _animators)
        {
            animator.SetTrigger(State.Hit.ToString());
        }
    }
}
