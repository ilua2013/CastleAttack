using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float _speed = 5f;

    private IMonstr _target;
    protected int _damage = 2;

    private void Start()
    {
        transform.SetParent(null);
    }

    public void Init(IMonstr monstr)
    {
        _target = monstr;
    }

    private void Update()
    {
        if (_target.TransformPosition == null)
            Destroy(gameObject);

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
