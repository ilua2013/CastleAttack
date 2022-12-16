using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ForceObject : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private Rigidbody _rigidbody;

    private Collider _collider;

    public Rigidbody Rigidbody => _rigidbody;

    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void Force(float force, Vector3 direction)
    {
        StartCoroutine(Live());
        _rigidbody.AddForce(direction * force);
    }

    private IEnumerator Live()
    {
        yield return new WaitForSeconds(_lifeTime);

        _collider.enabled = false;
        Destroy(gameObject, 2f);
    }
}
