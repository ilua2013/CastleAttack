using System.Collections;
using System.Collections.Generic;
using TypesMobs;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
public class Tower : MonoBehaviour, IMob
{
    [SerializeField] private int _radiusAttack;
    [SerializeField] private int _healt;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _reloadInterval;
    [SerializeField] private Transform _spawnPointProjectile;
    [SerializeField] private TypeMob _typesMobs;

    private CapsuleCollider _collider;
    private List<IMonstr> _monsters = new List<IMonstr>();

    public Vector3 TransformPosition => transform.position;

    public TypeMob TypeMob => _typesMobs;

    public event UnityAction Reloaded;
    public event UnityAction<IMob> CameOut;
    public event UnityAction<IMob> Deaded;



    private void Start()
    {
        StartCoroutine(Spawn());
        _collider = GetComponent<CapsuleCollider>();
        _collider.radius = _radiusAttack;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMonstr triggered))
        {
            _monsters.Add(triggered);           
            triggered.CameOut += StopAttack;
            triggered.Deaded += StopAttack;
        }
    }

    public void TakeDamage(int damage)
    {
        _healt -= damage;
        if (_healt <= 0)
        {
            Destruction();
        }
    }

    private void Destruction()
    {
        Deaded?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void StopAttack(IMonstr monstr)
    {
        _monsters.Remove(monstr);
        monstr.CameOut -= StopAttack;
        monstr.Deaded -= StopAttack;
    }

    private IEnumerator Spawn()
    {       
        float time = 0;

        while (time < _reloadInterval)
        {
            time += Time.deltaTime;
            yield return null;
        }
        if (_monsters.Count > 0)
        {
            Projectile projectile = Instantiate(_projectile, _spawnPointProjectile);
            projectile.Init(_monsters[0]);
        }
        StartCoroutine(Spawn());
    }
}