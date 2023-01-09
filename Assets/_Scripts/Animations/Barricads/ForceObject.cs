using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceObject : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private Rigidbody[] _rigidbody;
    [SerializeField] private ParticleSystem _particleExploision;

    private Collider[] _colliders;

    private void OnValidate()
    {
        _rigidbody = GetComponentsInChildren<Rigidbody>();
        _particleExploision = GetComponentInChildren<ParticleSystem>();

        foreach (var item in _rigidbody)
            item.isKinematic = true;
    }

    private void Awake()
    {
        _colliders = GetComponentsInChildren<Collider>();
    }

    public void Force(float force, Vector3 direction, Vector3 positionForce)
    {
        StartCoroutine(Live());

        _particleExploision.Play();

        foreach (var item in _colliders)
            item.enabled = true;

        foreach (var item in _rigidbody)
        {
            item.isKinematic = false;
            //item.AddForce(direction * force);
            //item.AddForceAtPosition(direction * force, item.transform.position + Random.insideUnitSphere);
            //item.AddRelativeForce(Random.insideUnitSphere);
            item.AddExplosionForce(force, positionForce, 100f);
        }
    }

    private IEnumerator Live()
    {
        yield return new WaitForSeconds(_lifeTime);

        foreach (var item in _colliders)
            item.enabled = false;

        Destroy(gameObject, 2f);
    }
}
