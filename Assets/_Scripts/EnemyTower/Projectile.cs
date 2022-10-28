using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _speed = 5f;

    private IMonstr _target;
    private IMob _source;
    protected int _damage = 2;
    private bool _isTargetActiv = false;

    private void OnDisable()
    {
        _source.Deaded -= NoSource;
        _target.Returned -= NoTarget;
    }

    private void Start()
    {
        transform.SetParent(null);
    }

    public void Init(IMonstr monstr, IMob mob)
    {
        _target = monstr;
        _isTargetActiv = true;
        _source = mob;
        _source.Deaded += NoSource;
        _target.Returned += NoTarget;
    }

    private void NoSource(IMob mob)
    {
       
        Destroy(gameObject);
    }

    private void NoTarget()
    {
        _isTargetActiv = false;       
        Destroy(gameObject);            
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.TransformPosition, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMonstr triggered))
        {           
            DamageEnemy(triggered);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Tower triggered))
        {
            Destroy(gameObject);
        }
    }

    protected virtual void DamageEnemy(IMonstr monstr)
    {       
        monstr.TakeDamage(_damage);      
        Destroy(gameObject);
    }
}
