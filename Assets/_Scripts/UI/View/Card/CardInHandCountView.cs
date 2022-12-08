using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CardInHandCountView : MonoBehaviour
{
    [SerializeField] private CardsHand _cardsHand;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        _text.text = $"{_cardsHand.CardsCount} / {_cardsHand.Capacity}";
    }
}
