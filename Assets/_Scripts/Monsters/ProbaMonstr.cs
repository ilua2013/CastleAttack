using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProbaMonstr : MonoBehaviour, IMonstr
{
    private int _healt = 10;
    private int _damage = 10;
    private int _speed = 2;

    public Vector3 TransformPosition => transform.position;
    public event UnityAction<IMonstr> CameOut;
    public event UnityAction<IMonstr> Deaded;

    public void TakeDamage(int damage)
    {
        _healt -= damage;
        if (_healt <= 0)
        {
            Deaded?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent(out Tower triggered))
    //    {
    //        triggered.TakeDamage(_damage);
    //    }
    //}
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Tower triggered))
        {
            CameOut?.Invoke(this);           
        }
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
