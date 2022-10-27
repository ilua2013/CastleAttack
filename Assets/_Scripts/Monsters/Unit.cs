using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using UnityEngine.UI;
using TypesMobs;
using System.Linq;

public class Unit : MonoBehaviour, IMonstr, IUnit
{
    [SerializeField] private TriggerZoneMonster _zoneMonster;
    [SerializeField] private float _reloadAttackInterval=2;
    [SerializeField] private float _speedAttack = 5;
    [SerializeField] private int _maxHealth = 40;
    [SerializeField] private Damage[] _damages;

    private int _health;
    private Coroutine _coroutine;
    private Transform _targetPoint;
    private Card _card;
    private NavMeshAgent _meshAgent;
    private Button _button;
    private IMob _target;
    private bool _isActivAttack = false;
    private bool _isMove = false;

    public Vector3 TransformPosition => transform.position;
    public Card Card => _card;

    public Transform TransformPoint => _targetPoint;
    public int Health => _health;
    public int MaxHealth => _maxHealth;

    public event UnityAction<IMonstr, IUnit> CameOut;
    public event UnityAction<IMonstr, IUnit> Deaded;
    public event UnityAction<IMonstr, int> Damaged;
    public event UnityAction<IMonstr, int> Healed;

    private void OnEnable()
    {
        _health = _maxHealth;
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

    public void Init(Card card, Transform transformPoint, Button button)
    {
        _card = card;
        _targetPoint = transformPoint;
        _button = button;
    }

    public void SetTargetPoint(Transform transform)
    {
        _targetPoint = transform;
    }

    private int CalculateDealtDamage(TypeMob mobe)
    {
        Damage damage = _damages.FirstOrDefault((damage)
                    => damage.TypeMob == mobe);

        return damage.Dealt;
    }

    private int CalculateTakenDamage(TypeMob mobe)
    {
        Damage damage = _damages.FirstOrDefault((damage)
                    => damage.TypeMob == mobe);

        return damage.Taken;
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

    private void Moved()
    {
        _isMove = true;
    }

    public void TakeDamage(int damage)
    {
        int totalDamage = CalculateTakenDamage(_target.TypeMob);

        _health -= totalDamage;
        Damaged?.Invoke(this, totalDamage);

        if (_health <= 0)
        {
            Deaded?.Invoke(this, this);
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
                _target.TakeDamage(CalculateDealtDamage(_target.TypeMob));
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
                _meshAgent.SetDestination(_target.TransformPosition);
                //transform.position = Vector3.MoveTowards(transform.position, _target.TransformPosition, _speedAttack * Time.deltaTime);
            }
            else
            {
                _meshAgent.SetDestination(_targetPoint.position);
            }

        }
        
    }

    public void ReurnToHand()
    {
        Destroy(gameObject);
    }

    public void RecoveryHealth(int amount)
    {
        _health = Mathf.Clamp(_health + amount, 0, _maxHealth);
        Healed?.Invoke(this, amount);
    }
}
