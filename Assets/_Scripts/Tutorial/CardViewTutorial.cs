using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardViewTutorial : MonoBehaviour
{
    [SerializeField] private CardLevelUp _cardLevelUpOld;
    [SerializeField] private CardLevelUp _cardLevelUpNew;

    public void OnDrawOut(UnitCard cardOld, UnitCard cardNew)
    {
        _cardLevelUpOld.ChangeDrawCard(cardOld);
        _cardLevelUpNew.ChangeDrawCard(cardNew);
    }    
}
