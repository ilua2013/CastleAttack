using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;

    public Sprite Background => _background.sprite;
    public Sprite Icon => _icon.sprite;
    public TMP_Text Text => _text;
    public TMP_Text AmountText => _amountText;
}
