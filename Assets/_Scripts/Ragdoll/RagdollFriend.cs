using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollFriend : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private UnitFriend _unitFriend;
    [SerializeField] private float _timeDelay;
    [SerializeField] private GameObject _unit;
    [SerializeField] private List<Rigidbody> _rigidbodies;
    [SerializeField] private Rigidbody _rigidbodyPlatform;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Animator _animatorPlatform;

    private void Start()
    {
        
        foreach (var rigi in _rigidbodies)
        {
            rigi.isKinematic = true;
        }
        _rigidbodyPlatform.isKinematic = true;
    }

    private void OnEnable()
    {
        _particleSystem.Stop();
        _unitFriend.Fighter.EffectDied += RagDollEnable;
    }

    private void OnDisable()
    {
        _unitFriend.Fighter.EffectDied -= RagDollEnable;
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
        Destroy(_unit);
    }
}