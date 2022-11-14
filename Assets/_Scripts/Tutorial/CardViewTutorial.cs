using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardViewTutorial : MonoBehaviour
{
    //[SerializeField] private Transform _cardPlacementOld;

    //[SerializeField] private Transform _cardPlacementNew;

    [SerializeField] private CardsDescription _cardsDescription;



    public void OnDrawOut(UnitCard cardOld, UnitCard cardNew)
    {
        _cardsDescription.CheckUnit(cardOld);
        Debug.Log("hhh");
       // //Card newCard = Instantiate(cardOld, transform);

       //cardOld.transform.SetParent(_cardPlacementOld);
       // cardOld.transform.localPosition = Vector3.zero;

       // //cardNew.transform.SetParent(_cardPlacementNew);
       // //cardNew.transform.localPosition = Vector3.zero;


    }
}
