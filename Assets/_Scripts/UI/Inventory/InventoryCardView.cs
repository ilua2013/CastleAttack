using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCardView : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _text;

    public void FillCard(Card card)
    {
        _background.enabled = false;
        _text.enabled = false;
        _icon.enabled = false;

        card.gameObject.SetActive(true);
        card.transform.SetParent(transform);
        card.transform.SetAsFirstSibling();
        card.transform.localPosition = Vector3.zero;
    }
}