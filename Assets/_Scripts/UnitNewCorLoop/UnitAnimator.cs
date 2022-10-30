using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private UnitStep _unitStep;    

    enum State
    {
        Attack, Move
    }   

    private void OnEnable()
    {
        _unitStep.Moved += SetMove;       
        _unitStep.Attacked += SetAttack;
    }

    private void OnDisable()
    {
        _unitStep.Moved -= SetMove;
        _unitStep.Attacked -= SetAttack;
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
