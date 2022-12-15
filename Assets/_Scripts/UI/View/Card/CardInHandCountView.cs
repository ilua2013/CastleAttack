using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInHandCountView : MonoBehaviour
{
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private TMP_Text _textMax;
    [SerializeField] private TMP_Text _text;

    private void OnEnable()
    {
        _cardsHand.CardAdded += OnCardTaken;
        _cardsHand.CardDrop += OnCardTaken;
    }

    private void OnDisable()
    {
        _cardsHand.CardAdded -= OnCardTaken;
        _cardsHand.CardDrop -= OnCardTaken;
    }

    private void OnCardTaken()
    {
        if (_cardsHand.CardsCount == _cardsHand.Capacity)
        {
            _textMax.enabled = true;
            _text.enabled = false;
        }
        else
        {
            _textMax.enabled = false;
            _text.enabled = true;
        }

        _text.text = $"{_cardsHand.CardsCount} / {_cardsHand.Capacity}";
    }
}
