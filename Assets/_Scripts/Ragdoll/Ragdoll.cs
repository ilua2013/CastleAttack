using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private UnitEnemy _unitEnemy;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _transformDown;
    [SerializeField] private GameObject _unit;
    [SerializeField] private List<Rigidbody> _rigidbodies;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animatorPlatform;  

    private void Start()
    {
        foreach (var rigi in _rigidbodies)
        {
            rigi.isKinematic = true;
        }
        _rigidbody.isKinematic = true;
    }

    private void OnEnable()
    {
        _unitEnemy.Fighter.Died += RagDollEnable;
    }

    private void OnDisable()
    {
        _unitEnemy.Fighter.Died -= RagDollEnable;
    }

    private void RagDollEnable()
    {
        foreach (var rigi in _rigidbodies)
        {
            rigi.isKinematic = false;
        }
        _rigidbody.isKinematic = false;
        _animatorPlatform.enabled = false;
        _animator.enabled = false;
        foreach (var rigi in _rigidbodies)
        {
            rigi.AddForce(Vector3.up * 1100);
        }
        _rigidbody.AddForce(Vector3.up * 500);
        StartCoroutine(Delay());
    } 

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(7f);
        foreach (var rigi in _rigidbodies)
        {
            rigi.isKinematic = true;
        }
        _rigidbody.isKinematic = true;
        StartCoroutine(Down());
        
    }


    private IEnumerator Down()
    {
        Vector3 startPos = _unit.transform.position;
        Vector3 targetPos = _transformDown.position - _unit.transform.position;
        float time = 0;

        while (time < _speed)
        {
            time = time > _speed ? _speed : time + Time.deltaTime;

           _unit.transform.position = startPos + (targetPos * (time / _speed));
           
            yield return null;
        }
       _unit.SetActive(false);

    }
}
