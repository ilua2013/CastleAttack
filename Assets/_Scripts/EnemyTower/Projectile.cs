using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _speed = 5f;

    private IMonstr _target;
    protected int _damage = 2;
    private bool _isTargetActiv = false;

    private void Start()
    {
        transform.SetParent(null);
    }

    private void OnDisable()
    {
       
    }

    public void Init(IMonstr monstr)
    {
        _isTargetActiv = true;
        _target = monstr;
        _target.Returned += NoTarget;
    }

    private void NoTarget()
    {
        _isTargetActiv = false;
        _target = null;
        Debug.Log("yyyy");
    }

    private void Update()
    {
        if (_isTargetActiv == false)
        {
            
            Destroy(gameObject);
            return;
        }
           

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
        _target.Returned -= NoTarget;
        monstr.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
