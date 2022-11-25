using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private UnitEnemy _unitEnemy;
    [SerializeField] private float _timeDelay;    
    [SerializeField] private GameObject _unit;
    [SerializeField] private List<Rigidbody> _rigidbodies;
    [SerializeField] private Rigidbody _rigidbodyPlatform;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Animator _animatorPlatform;  

    private void Start()
    {
        _particleSystem.Stop();
        foreach (var rigi in _rigidbodies)
        {
            rigi.isKinematic = true;
        }
        _rigidbodyPlatform.isKinematic = true;
    }

    private void OnEnable()
    {
        _unitEnemy.Fighter.EffectDied += RagDollEnable;
    }

    private void OnDisable()
    {
        _unitEnemy.Fighter.EffectDied -= RagDollEnable;
    }

    private void RagDollEnable()
    {
        foreach (var rigi in _rigidbodies)
        {
            rigi.isKinematic = false;
        }
        _rigidbodyPlatform.isKinematic = false;
        _animatorPlatform.enabled = false;
        _animator.enabled = false;      
        foreach (var rigi in _rigidbodies)
        {
            rigi.AddForce(Vector3.up * 800);
        }
        _particleSystem.Play();
        StartCoroutine(DelayDied());
    } 
    
    private IEnumerator DelayDied()
    {
        yield return new WaitForSeconds(_timeDelay);

        GamesStatistics.RegisterEnemyKill();
        Destroy(_unit);
    }
}
