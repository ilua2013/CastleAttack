using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MobTriggeredZone : MonoBehaviour
{
    private List<IMonstr> _monstres = new List<IMonstr>();

    public List<IMonstr> Monstres => _monstres;

    public event UnityAction<IMonstr> Entered;

    private bool _isActivEnemy = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMonstr triggered))
        {
            if (_isActivEnemy == false)
            {
                Entered?.Invoke(triggered);
                _isActivEnemy = true;
            }

            _monstres.Add(triggered);
            triggered.Deaded += StopAttack;
        }
    }

    private void StopAttack(IMonstr mob, IUnit unit)
    {
        _monstres.Remove(mob);

        if (_monstres.Count == 0)
        {
            _isActivEnemy = false;
        }
        mob.Deaded -= StopAttack;
    }






















    //[SerializeField] private Mob _mob;

    ////private List<IMonstr> _monsters = new List<IMonstr>();

    //public event UnityAction<IMonstr> Attacked;
    //public event UnityAction Entered;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent(out IMonstr triggered))
    //    {
    //        Attacked?.Invoke(triggered);
    //        Entered?.Invoke();
    //        //_monsters.Add(triggered);
    //        //_mob.Init(_monsters[0]);
    //        //triggered.Deaded += StopAttack;
    //    }
    //}

    ////private void StopAttack(IMonstr monstr)
    ////{

    ////    _monsters.Remove(monstr);
    ////    monstr.Deaded -= StopAttack;
    ////    if (_monsters.Count > 0)
    ////    {
    ////        _mob.Init(_monsters[0]);
    ////    }


    ////}
}
