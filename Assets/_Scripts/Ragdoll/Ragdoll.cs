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

    private Rigidbody[] _rigidbodysAll;

    private void OnValidate()
    {
        if (GetComponentInParent<Platform>() != null)
        {
            _rigidbodyPlatform = GetComponentInParent<Platform>().GetComponent<Rigidbody>();
            _animatorPlatform = GetComponentInParent<Platform>().GetComponent<Animator>();
        }
    }

    private void Start()
    {
        _particleSystem.Stop();
        foreach (var rigi in _rigidbodies)
        {
            rigi.isKinematic = true;
        }

        if (GetComponentInParent<Platform>() != null)
        {
            _rigidbodyPlatform = GetComponentInParent<Platform>().GetComponent<Rigidbody>();
            _animatorPlatform = GetComponentInParent<Platform>().GetComponent<Animator>();
        }

        _rigidbodyPlatform.isKinematic = true;

        _rigidbodysAll = GetComponentsInChildren<Rigidbody>();
    }

    private void OnEnable()
    {
        _unitEnemy.Fighter.EffectDied += RagDollEnable;
    }

    private void OnDisable()
    {
        _unitEnemy.Fighter.EffectDied -= RagDollEnable;
    }

    private void RagDollEnable(Vector3 force)
    {
        //foreach (var rigi in _rigidbodies)
        //{
        //    rigi.isKinematic = false;
        //}
        //_rigidbodyPlatform.isKinematic = false;
        //_animatorPlatform.enabled = false;
        //_animator.enabled = false;

        //Invoke(nameof(EnableRigidbodyPlatform), 1f);

        //foreach (var rigi in _rigidbodies)
        //{
        //    // rigi.AddForce(Vector3.up * 800);
        //    rigi.AddForce(force);
        //}
        _particleSystem.Play();
        StartCoroutine(DelayDied());
    }

    public void RagDollEnable()
    {
        //foreach (var rigi in _rigidbodies)
        //    rigi.isKinematic = false;

        //foreach (var item in _rigidbodysAll)
        //{
        //    item.isKinematic = false;
        //    item.useGravity = true;
        //}

        //_rigidbodyPlatform.isKinematic = false;
        //_animatorPlatform.enabled = false;
        //_animator.enabled = false;

        //Invoke(nameof(EnableRigidbodyPlatform), 1f);

        //foreach (var rigi in _rigidbodies)
        //{
        //     rigi.AddForce(Vector3.up * 800);
        //}
        _particleSystem.Play();
        StartCoroutine(DelayDied());
    }

    private IEnumerator DelayDied()
    {
        yield return new WaitForSeconds(_timeDelay);

        GamesStatistics.RegisterEnemyKill();
        Destroy(_unit);
    }

    private void EnableRigidbodyPlatform()
    {
        _rigidbodyPlatform.isKinematic = false;
        _rigidbodyPlatform.useGravity = true;
    }
}
