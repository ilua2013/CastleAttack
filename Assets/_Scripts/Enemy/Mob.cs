using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private int _healt;
    [SerializeField] private MobTriggeredZone _mobTriggeredZone;
    [SerializeField] private MobEnemies _mobEnemy;
    private IMonstr _target = null;
    protected int _damage = 2;
    private bool _isAttack = false;
    private int _index = 0;

    private void OnEnable()
    {
        _mobTriggeredZone.Entered += Init;
    }

    private void OnDisable()
    {
        _mobTriggeredZone.Entered -= Init;
    }

    public void Init()
    {

        if (_isAttack == false)
        {
            if (_mobEnemy.Monsters[_index] != null)
            {
                _target = _mobEnemy.Monsters[_index];
                _isAttack = true;
                ++_index;
                _target.Deaded += StopAttack;
            }
        }
    }

    private void Update()
    {
        if (_target != null)
        {
            float distance = Vector3.Distance(transform.position, _target.TransformPosition);

            if (distance > 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target.TransformPosition, _speed * Time.deltaTime);
            }
        }
    }

    private void StopAttack(IMonstr monstr)
    {
        _isAttack = false;
        Init();
        _target.Deaded -= StopAttack;
    }

    public void TakeDamage(int damage)
    {
        _healt -= damage;
        if (_healt <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMonstr monstr))
        {
            DamageEnemy(_target);
        }
    }

    private void DamageEnemy(IMonstr monstr)
    {
        monstr.TakeDamage(_damage);
    }
}
