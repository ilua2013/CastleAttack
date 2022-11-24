using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardRewardView : MonoBehaviour
{
    private readonly Vector3 _offset = new Vector3(4.3f, 3f, 0);

    [SerializeField] private Transform _cardContainer;
    [SerializeField] private TMP_Text _amountText;

    public void Fill(Card card, int amount)
    {
        card.gameObject.SetActive(true);
        card.transform.SetParent(_cardContainer);

        card.transform.localScale = Vector3.one;
        card.transform.localPosition = _offset;

        _amountText.text = $"x{amount}";
    }
}
