using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInDeckView : MonoBehaviour
{
    [SerializeField] private Slider _amountBar;
    [SerializeField] private TMP_Text _amountText;

    [SerializeField] private bool _isMoveable;
    [SerializeField] private CardName _cardName;

    private readonly Vector3 _initialScale = new Vector3(2f, 2f, 2f);

    private Shop _shop;
    private Card _card;

    public Card Card => _card;
    public CardName CardName => _cardName;

    private void Awake()
    {
        _shop = FindObjectOfType<Shop>();
    }

    private void OnEnable()
    {
        _shop.CardBought += OnCardBought;
        _shop.CardOpened += OnCardOpened;
    }

    private void OnDisable()
    {
        _shop.CardBought -= OnCardBought;
        _shop.CardOpened -= OnCardOpened;
    }

    public bool TryLevelUpCard()
    {
        if (_card == null)
            return false;

        if (_card.CardSave.CanLevelUp)
        {
            _card.CardSave.LevelUp();
            _card.Save();
            SetInfo(_card.CardSave);

            return true;
        }

        return false;
    }

    public void Clear()
    {
        _card = null;

        _amountText.text = "0 / 3";
        _amountBar.value = 0;
    }

    public void FillCard(Card card, bool smooth)
    {
        _card = card;
        _card.gameObject.SetActive(true);
        
        if (IsNotUnit())
            _amountBar.gameObject.SetActive(_card.CardSave.IsAvailable);

        _card.Activate(_isMoveable);

        SetHierarchy(card.transform);
        Transformation(card.transform, smooth);
        SetInfo(card.CardSave);

        card.transform.localScale = Vector3.one;
    }

    private void SetInfo(CardSave cardSave)
    {
        _amountBar.wholeNumbers = true;
        _amountBar.minValue = 0;
        _amountBar.maxValue = cardSave.AmountToImprove;

        _amountBar.value = cardSave.Amount;
        _amountText.text = $"{cardSave.Amount} / {cardSave.AmountToImprove}";
    }

    private void SetHierarchy(Transform card)
    {
        card.SetParent(transform);
        //card.SetAsFirstSibling();
    }

    private void Transformation(Transform card, bool smooth)
    {
        card.localScale = _initialScale;
        card.rotation = Quaternion.identity;
        card.localPosition = Vector3.zero;
    }

    private void OnCardBought(Card card)
    {
        if (_card == null)
            return;

        if (card.Name == _card.Name)
            SetInfo(card.CardSave);
    }

    private void OnCardOpened(Card card)
    {
        if (card.Name == _cardName)
        {
            _card = card;
            _card.Save(card.CardSave);

            FillCard(_card, false);
        }
    }

    private bool IsNotUnit()
    {
        return CardName != CardName.Hand && CardName != CardName.Ork && CardName != CardName.Snake && CardName != CardName.Bat;
    }
}