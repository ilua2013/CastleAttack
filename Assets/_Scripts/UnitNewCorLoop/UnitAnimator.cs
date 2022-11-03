using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private IUnit _unit;
    private Mover _mover;
    private Fighter _fighter;

    enum State
    {
        Attack, Move
    }

    private void Awake()
    {
        _unit = GetComponent<IUnit>();
        _mover = _unit.Mover;
        _fighter = _unit.Fighter;
    }

    private void OnEnable()
    {
        _mover.Moved += SetMove;
        _fighter.Attacked += SetAttack;
    }

    private void OnDisable()
    {
        _mover.Moved -= SetMove;
        _fighter.Attacked -= SetAttack;
    }   

    private void SetAttack()
    {
        _animator.SetTrigger(State.Attack.ToString());        
    }

    private void SetMove()
    {       
        _animator.SetTrigger(State.Move.ToString());        
    }
}
