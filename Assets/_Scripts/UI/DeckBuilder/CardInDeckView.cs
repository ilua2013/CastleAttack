using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInDeckView : MonoBehaviour
{
    private readonly Vector3 _initialScale = new Vector3(1.5f, 1.5f, 1.5f);

    public void FillCard(Card card)
    {
        card.transform.SetParent(transform);
        card.transform.SetAsFirstSibling();

        card.transform.localScale = _initialScale;
        card.transform.rotation = Quaternion.identity;
        card.transform.localPosition = Vector3.zero;
    }
}