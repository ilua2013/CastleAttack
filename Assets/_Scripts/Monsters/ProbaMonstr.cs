using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;

public class ProbaMonstr : MonoBehaviour, IMonstr, IUnit
{
   
    [SerializeField] private TriggerZoneMonster _zoneMonster;
    [SerializeField] private float _reloadAttackInterval=2;
    [SerializeField] private float _speedAttack = 5;
    [SerializeField] private int _healt = 40;
    [SerializeField] private int _damageTower = 10;
    [SerializeField] private int _damageSecurity = 20;

    private Coroutine _coroutine;
    private Transform _targetPoint;
    private NavMeshAgent _meshAgent;
    private Button _button;
    private IMob _target;
    private bool _isActivAttack = false;
    private bool _isMove = false;

    public Vector3 TransformPosition => transform.position;

    public Transform TransformPoint => _targetPoint;

    public event UnityAction<IMonstr> CameOut;
    public event UnityAction<IMonstr> Deaded;

    private void OnEnable()
    {
        _zoneMonster.Entered += StartAttack;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(Moved);
        _zoneMonster.Entered -= StartAttack;
    }   

    private void Start()
    {
        _button.onClick.AddListener(Moved);
        _meshAgent = GetComponent<NavMeshAgent>();
    }

    public void Init(Transform transformPoint, Button button)
    {
        _targetPoint = transformPoint;
        _button = button;
        _button.onClick.AddListener(Moved);
    }

    public void SetTargetPoint(Transform transform)
    {
        _targetPoint = transform;
    }

    private void StartAttack(IMob mob)
    {
        mob.Deaded += ContinionAttack;
        _target = mob;
        _isActivAttack = true;
        _coroutine = StartCoroutine(Attack());
        Debug.Log(mob);
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

    private void Moved()
    {
        _isMove = true;
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
            int damage;
            float distance = Vector3.Distance(transform.position, _target.TransformPosition);
            if (distance < 3)
            {
                if(_target.TypeMob == TypesMobs.TypeMob.Security)
                {
                    damage = _damageSecurity;
                }
                else
                {
                    damage = _damageTower;
                }
                _target.TakeDamage(damage);
            }
        }
        StartCoroutine(Attack());
    }

    private void Update()
    {
        if(_isMove == true)
        {
            if (_isActivAttack == true)
            {

                transform.position = Vector3.MoveTowards(transform.position, _target.TransformPosition, _speedAttack * Time.deltaTime);
            }
            else
            {
                _meshAgent.SetDestination(_targetPoint.position);
            }

        }
        
    }
}
