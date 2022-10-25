using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProbaMonstr : MonoBehaviour, IMonstr
{
    [SerializeField] private Transform _transformPoint;
    [SerializeField] private TriggerZoneMonster _zoneMonster;
    [SerializeField] private float _reloadInterval;
    [SerializeField] private Collider _collider;

    private int _healt = 30;
    private int _damage = 10;
    private int _speed = 2;

    public Vector3 TransformPosition => transform.position;
    public event UnityAction<IMonstr> CameOut;
    public event UnityAction<IMonstr> Deaded;

    private void Start()
    {
        //StartCoroutine(Attack());
    }

    public void TakeDamage(int damage)
    {
        _healt -= damage;
        if (_healt <= 0)
        {
            Deaded?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tower triggered))
        {
            triggered.TakeDamage(_damage);
        }
    }
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent(out Tower triggered))
    //    {
    //        CameOut?.Invoke(this);           
    //    }
    //}
    private void Update()
    {
        if (_zoneMonster.Mobes.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, _zoneMonster.Mobes[0].TransformPosition, 5 * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _transformPoint.transform.position, _speed * Time.deltaTime);
        }
        

        //transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    //private IEnumerator Attack()
    //{
        
    //    float time = 0;

    //    while (time < _reloadInterval)
    //    {
    //        time += Time.deltaTime;
    //        yield return null;
    //    }
    //    if (_zoneMonster.Mobes.Count > 0)
    //    {
           
    //    }
    //    StartCoroutine(Attack());
    //}
}
