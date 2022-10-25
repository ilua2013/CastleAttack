using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class ProbaMonstr : MonoBehaviour, IMonstr, IUnit
{
    [SerializeField] private Transform _transformPoint;
    [SerializeField] private TriggerZoneMonster _zoneMonster;
    [SerializeField] private float _reloadAttackInterval=2;
    [SerializeField] private float _speedAttack = 5;
    [SerializeField] private int _healt = 40;
    [SerializeField] private int _damage = 10;

    private Coroutine _coroutine;

    private NavMeshAgent _meshAgent;
    private IMob _target;
    private bool _isActivAttack = false;

    public Vector3 TransformPosition => transform.position;

    public Transform TransformPoint => _transformPoint;

    public event UnityAction<IMonstr> CameOut;
    public event UnityAction<IMonstr> Deaded;

    private void OnEnable()
    {
        _zoneMonster.Entered += StartAttack;
    }

    private void OnDisable()
    {
        _zoneMonster.Entered -= StartAttack;
    }

    public void Init()
    {
        
    }

    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
    }

    public void Init(Transform transformPoint)
    {
        _transformPoint = transformPoint;
    }

    private void StartAttack(IMob mob)
    {
        mob.Deaded += ContinionAttack;
        _target = mob;
        _isActivAttack = true;
        _coroutine = StartCoroutine(Attack());
    }

    private void ContinionAttack(IMob mob)
    {
        if (_zoneMonster.Mobes.Count > 0)
        {
            Debug.Log(_zoneMonster.Mobes[_zoneMonster.Mobes.Count - 1]);
            _target = _zoneMonster.Mobes[_zoneMonster.Mobes.Count - 1];
            _target.Deaded += ContinionAttack;
        }
        else
        {
            StopAttack();
        }
        mob.Deaded -= ContinionAttack;
    }

    private void StopAttack()
    {
        _target.Deaded -= ContinionAttack;
        _target = null;
        _isActivAttack = false;
        StopCoroutine(_coroutine);
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

    private IEnumerator Attack()
    {
        float time = 0;

        while (time < _reloadAttackInterval)
        {
            time += Time.deltaTime;
            yield return null;
        }
        if (_isActivAttack == true)
        {
            float distance = Vector3.Distance(transform.position, _target.TransformPosition);
            if (distance < 2)
            {
                _target.TakeDamage(_damage);
            }
        }
        StartCoroutine(Attack());
    }

    private void Update()
    {
        if (_isActivAttack == true)
        {

            transform.position = Vector3.MoveTowards(transform.position, _target.TransformPosition, _speedAttack * Time.deltaTime);
        }
        else
        {
            _meshAgent.SetDestination(_transformPoint.position);
        }
    }
}
