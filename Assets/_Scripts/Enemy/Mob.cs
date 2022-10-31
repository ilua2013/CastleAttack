using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using TypesMobs;

public class Mob : MonoBehaviour, IMob
{
    //[SerializeField] private Transform _transformPoint;
    [SerializeField] private MobTriggeredZone _zoneMob;
    [SerializeField] private float _reloadAttackInterval=2;
    [SerializeField] private float _speedAttack = 5;
    [SerializeField] private int _healt = 40;
    [SerializeField] private int _maxHealt = 40;
    [SerializeField] private int _damage = 10;

    [SerializeField] private TypeMob _typesMobs;

    private Coroutine _coroutine;

    private NavMeshAgent _meshAgent;
    private IMonstr _target;
    private bool _isActivAttack = false;

    public Vector3 TransformPosition => transform.position;
    public TypeMob TypeMob => _typesMobs;
    public int Health => _healt;
    public int MaxHealth => _maxHealt;

    public event UnityAction<IMob> CameOut;
    public event UnityAction<IMob> Deaded;
    public event UnityAction<int> Damaged;
    public event UnityAction<int> Healed;

    private void OnEnable()
    {
        _zoneMob.Entered += StartAttack;
    }

    private void OnDisable()
    {
        _zoneMob.Entered -= StartAttack;

    }

    private void Start()
    {
        _meshAgent = GetComponent<NavMeshAgent>();
    }

    private void StartAttack(IMonstr mob)
    {
        mob.Deaded += ContinionAttack;
        _target = mob;
        _isActivAttack = true;
        _coroutine = StartCoroutine(Attack());
    }

    private void ContinionAttack(IMonstr mob, IUnit unit)
    {
        if (_zoneMob.Monstres.Count > 0)
        {
            Debug.Log(_zoneMob.Monstres[_zoneMob.Monstres.Count - 1]);
            _target = _zoneMob.Monstres[_zoneMob.Monstres.Count - 1];
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
        Damaged?.Invoke(damage);

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
                //_target.TakeDamage(_damage);
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
            
        }
    }















    //[SerializeField] private float _speed = 2f;
    //[SerializeField] private int _healt;
    //[SerializeField] private MobTriggeredZone _mobTriggeredZone;
    //[SerializeField] private MobEnemies _mobEnemy;
    //private IMonstr _target = null;
    //protected int _damage = 2;
    //private bool _isAttack = false;
    //private int _index = 0;

    //private void OnEnable()
    //{
    //    _mobTriggeredZone.Entered += Init;
    //}

    //private void OnDisable()
    //{
    //    _mobTriggeredZone.Entered -= Init;
    //}

    //public void Init()
    //{

    //    if (_isAttack == false)
    //    {
    //        if (_mobEnemy.Monsters[_index] != null)
    //        {
    //            _target = _mobEnemy.Monsters[_index];
    //            _isAttack = true;
    //            ++_index;
    //            _target.Deaded += StopAttack;
    //        }
    //    }
    //}

    //private void Update()
    //{
    //    if (_target != null)
    //    {
    //        float distance = Vector3.Distance(transform.position, _target.TransformPosition);

    //        if (distance > 2)
    //        {
    //            transform.position = Vector3.MoveTowards(transform.position, _target.TransformPosition, _speed * Time.deltaTime);
    //        }
    //    }
    //}

    //private void StopAttack(IMonstr monstr)
    //{
    //    _isAttack = false;
    //    Init();
    //    _target.Deaded -= StopAttack;
    //}

    //public void TakeDamage(int damage)
    //{
    //    _healt -= damage;
    //    if (_healt <= 0)
    //    {
    //        gameObject.SetActive(false);
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent(out IMonstr monstr))
    //    {
    //        DamageEnemy(_target);
    //    }
    //}

    //private void DamageEnemy(IMonstr monstr)
    //{
    //    monstr.TakeDamage(_damage);
    //}
}
