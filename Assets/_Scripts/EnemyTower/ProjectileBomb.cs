using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBomb : Projectile
{
    [SerializeField] private int _radiusDefeat;

    protected override void DamageEnemy(IMonstr monstr)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radiusDefeat);
        foreach (var collider in hitColliders)
        {
            if (collider.TryGetComponent(out IMonstr triggered))
            {
                triggered.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
