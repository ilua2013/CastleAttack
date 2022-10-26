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
    [SerializeField] private int _damage = 10;

    private Coroutine _coroutine;
    private Transform _transformPoint;
    private Card _card;
    private NavMeshAgent _meshAgent;
    private Button _button;
    private IMob _target;
    private bool _isActivAttack = false;
    private bool _isMove = false;

    public Vector3 TransformPosition => transform.position;
    public Card Card => _card;

    public Transform TransformPoint => _transformPoint;

    public event UnityAction<IMonstr, IUnit> CameOut;
    public event UnityAction<IMonstr, IUnit> Deaded;

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

    public void Init(Card card, Transform transformPoint, Button button)
    {
        _card = card;
        _transformPoint = transformPoint;
        _button = button;
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
        _healt -= damage;
        if (_healt <= 0)
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
                _target.TakeDamage(_damage);
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
                _meshAgent.SetDestination(_transformPoint.position);
            }

        }
        
    }

    public void ReurnToHand()
    {
        Destroy(gameObject);
    }
}
