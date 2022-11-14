using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsDescription : MonoBehaviour
{
    [SerializeField] private UnitCard[] _unitCards;
    [SerializeField] private Transform _cardPlacementOld;

    public void CheckUnit(UnitCard unitCard)
    {
        foreach (var unit in _unitCards)
        {
            if(unit == unitCard)
            {
                Card newCard = Instantiate(unit, transform);
                newCard.transform.SetParent(_cardPlacementOld);
                newCard.transform.localPosition = Vector3.zero;
                Debug.Log("ggggggggg");
            }
        }
    }
}
