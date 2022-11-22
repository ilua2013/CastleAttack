using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardViewTutorial : MonoBehaviour
{
    [SerializeField] private CardLevelUp _cardLevelUpOld;
    [SerializeField] private CardLevelUp _cardLevelUpNew;

    private float _delayTime;
    public event Action EndStep;

    //public void OnDrawOut(UnitCard cardOld, UnitCard cardNew, float time )
    //{
    //    _delayTime = time;
    //    _cardLevelUpOld.ChangeDrawCard(cardOld);
    //    _cardLevelUpNew.ChangeDrawCard(cardNew);
      
    //}

    //public void EndViewCardUpgrade()
    //{
    //    EndStep?.Invoke();
    //    gameObject.SetActive(false);
    //}

    //private IEnumerator DisableWindows()
    //{        
    //    yield return new WaitForSeconds(_delayTime);
    //    EndStep?.Invoke();
    //    StopCoroutine(DisableWindows());
    //    gameObject.SetActive(false);
       
    //}
}
