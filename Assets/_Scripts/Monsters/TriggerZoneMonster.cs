using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZoneMonster : MonoBehaviour
{
    private List<IMob> _mobes = new List<IMob>();

    public List<IMob> Mobes => _mobes;

    public event UnityAction<IMob> Entered;

    private bool _isActivEnemy = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMob triggered))
        {
            if (_isActivEnemy == false)
            {
                Entered?.Invoke(triggered);
                _isActivEnemy = true;
            }

            _mobes.Add(triggered);
            triggered.Deaded += StopAttack;
        }
    }

    private void StopAttack(IMob mob)
    {
        _mobes.Remove(mob);

        if (_mobes.Count == 0)
        {
            _isActivEnemy = false;
        }
        mob.Deaded -= StopAttack;
    }
}
