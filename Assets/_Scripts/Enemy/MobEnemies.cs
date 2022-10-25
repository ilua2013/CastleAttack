//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class MobEnemies : MonoBehaviour
//{
//    [SerializeField] private Mob _mob;
//    [SerializeField] private MobTriggeredZone _triggeredZone;

//    private IMonstr _monstr1;    
//    private List<IMonstr> _monsters = new List<IMonstr>(); 

//    public List<IMonstr> Monsters => _monsters;
//    public IMonstr Monstr1 => _monstr1;

//    private void OnEnable()
//    {
//        _triggeredZone.Attacked += AddMonster;
//        foreach (var monstr in _monsters)
//        {
//            monstr.Deaded += RemoveMonstr;
//        }
//    }
//    private void OnDisable()
//    {
//        _triggeredZone.Attacked -= AddMonster;
//        foreach (var monstr in _monsters)
//        {
//            monstr.Deaded -= RemoveMonstr;
//        }
//    }

//    private void AddMonster(IMonstr monstr)
//    {
//        _monsters.Add(monstr);       
//    }

//    private void RemoveMonstr(IMonstr monstr)
//    {
//        _monsters.Remove(monstr);       
//    }
//}
