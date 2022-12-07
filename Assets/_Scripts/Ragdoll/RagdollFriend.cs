using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollFriend : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private UnitFriend _unitFriend;
    [SerializeField] private float _timeDelay;
    [SerializeField] private float _force = 850f;
    [SerializeField] private GameObject _unit;
    [SerializeField] private List<Rigidbody> _rigidbodies;
    [SerializeField] private Rigidbody _rigidbodyPlatform;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private Animator _animatorPlatform;

    private Rigidbody[] _rigidbodysAll;

    private void OnValidate()
    {
        foreach (var item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = true;
            item.mass = 1;
        }

        if (GetComponentInParent<Platform>() != null)
        {
            _rigidbodyPlatform = GetComponentInParent<Platform>().GetComponent<Rigidbody>();
            _animatorPlatform = GetComponentInParent<Platform>().GetComponent<Animator>();
        }
    }

    private void Start()
    {
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
        if(_particleSystem != null)
        _particleSystem.Stop();
        _unitFriend.Fighter.EffectDied += RagDollEnable;
    }

    private void OnDisable()
    {
        _unitFriend.Fighter.EffectDied -= RagDollEnable;
    }

    private void RagDollEnable()
    {
        //foreach (var rigi in _rigidbodies)
        //{
        //    rigi.isKinematic = false;
        //}

        //foreach (var item in _rigidbodysAll)
        //{
        //    item.isKinematic = false;
        //    item.useGravity = true;
        //}

        //_rigidbodyPlatform.isKinematic = false;
        //_animatorPlatform.enabled = false;
        //_animator.enabled = false;
        //foreach (var rigi in _rigidbodies)
        //{
        //    rigi.AddForce(Vector3.up * 800);
        //}
        _particleSystem.Play();
        StartCoroutine(DelayDied());
    }

    public void RagDollEnable(Fighter fighter)
    {
        //foreach (var rigi in _rigidbodies)
        //{
        //    rigi.isKinematic = false;
        //}

        //foreach (var item in _rigidbodysAll)
        //{
        //    item.isKinematic = false;
        //    item.useGravity = true;
        //}

        //_animatorPlatform.enabled = false;
        //_animator.enabled = false;

        //Invoke(nameof(EnableRigidbodyPlatform), 1f);

        //Vector3 forceFrom = transform.position - fighter.transform.position;
        //forceFrom.y = 5f;

        //foreach (var rigi in _rigidbodies)
        //    rigi.AddForce(forceFrom.normalized * _force);

        _particleSystem.Play();
        StartCoroutine(DelayDied());
    }

    private IEnumerator DelayDied()
    {
        yield return new WaitForSeconds(_timeDelay);

        GamesStatistics.RegisterFriendKill();
        Destroy(_unit);
    }

    private void EnableRigidbodyPlatform()
    {
        _rigidbodyPlatform.isKinematic = false;
        _rigidbodyPlatform.useGravity = true;
    }
}
