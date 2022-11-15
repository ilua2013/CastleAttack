using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCardView : MonoBehaviour
{
    public void FillCard(Card card)
    {
        card.gameObject.SetActive(true);
        card.transform.SetParent(transform);
        card.transform.SetAsFirstSibling();
        card.transform.localPosition = Vector3.zero;
    }
}