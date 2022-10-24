using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MobTriggeredZone : MonoBehaviour
{
    [SerializeField] private Mob _mob;

    //private List<IMonstr> _monsters = new List<IMonstr>();

    public event UnityAction<IMonstr> Attacked;
    public event UnityAction Entered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMonstr triggered))
        {
            Attacked?.Invoke(triggered);
            Entered?.Invoke();
            //_monsters.Add(triggered);
            //_mob.Init(_monsters[0]);
            //triggered.Deaded += StopAttack;
        }
    }

    //private void StopAttack(IMonstr monstr)
    //{

    //    _monsters.Remove(monstr);
    //    monstr.Deaded -= StopAttack;
    //    if (_monsters.Count > 0)
    //    {
    //        _mob.Init(_monsters[0]);
    //    }


    //}
}
