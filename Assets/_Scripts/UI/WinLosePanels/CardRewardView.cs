using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardRewardView : MonoBehaviour
{
    private readonly Vector3 _offset = new Vector3(4.3f, 3f, 0);

    [SerializeField] private Transform _cardContainer;
    [SerializeField] private TMP_Text _amountText;

    public Card Card { get; private set; }
    public int Amount { get; private set; }

    public void Fill(Card card, int amount)
    {
        Card = card;
        Amount = amount;

        gameObject.SetActive(true);

        card.gameObject.SetActive(true);
        card.transform.SetParent(_cardContainer);

        card.transform.localScale = Vector3.one * 1.5f;
        card.transform.localPosition = _offset;

        _amountText.text = $"x{amount}";
    }

    public void Clear()
    {
        gameObject.SetActive(false);
    }
}
