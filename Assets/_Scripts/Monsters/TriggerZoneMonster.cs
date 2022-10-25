using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZoneMonster : MonoBehaviour
{   
    private List<IMob> _mobes = new List<IMob>();

    public List<IMob> Mobes => _mobes;

    //public event UnityAction<IMob> Attacked;
    //public event UnityAction Entered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IMob triggered))
        {
            //Attacked?.Invoke(triggered);
            //Entered?.Invoke();
            Debug.Log("вход"+_mobes.Count);
            _mobes.Add(triggered);
            Debug.Log("после вход"+_mobes.Count);
            //_mob.Init(_monsters[0]);
            triggered.Deaded += StopAttack;
        }
    }

    private void StopAttack(IMob mob)
    {
        Debug.Log("до удаления" +_mobes.Count);
        _mobes.Remove(mob);
        Debug.Log("после удаления"+_mobes.Count);
        mob.Deaded -= StopAttack;
    }
}
