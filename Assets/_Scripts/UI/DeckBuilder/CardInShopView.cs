using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardInShopView : MonoBehaviour
{
    private readonly Vector3 _initialScale = new Vector3(1f, 1f, 1f);

    [SerializeField] private Button _buyButton;
    [SerializeField] private Image _buyButtonImage;
    [SerializeField] private Color _inactiveColor;
    [SerializeField] private int _cost;
    [SerializeField] private bool _isMoveable;
    [SerializeField] private CardName _cardName;

    private Color _defaultColor;
    private Card _card;
    private CoinsWallet _wallet;

    public Card Card => _card;
    public CardName CardName => _cardName;

    public event Action<Card> CardBought;
    public event Action<Card> CardOpened;

    private void Awake()
    {
        _wallet = FindObjectOfType<CoinsWallet>();
        _defaultColor = _buyButtonImage.color;
    }

    private void OnEnable()
    {
        _buyButton.onClick.AddListener(OnBuyClick);
        _wallet.CoinsChanged += OnCoinsChanged;
    }

    private void OnDisable()
    {
        _buyButton.onClick.RemoveListener(OnBuyClick);
        _wallet.CoinsChanged -= OnCoinsChanged;
    }

    public void FillCard(Card card, bool isNew)
    {
        _card = card;
        _card.gameObject.SetActive(_card.CardSave.IsAvailable);
        _buyButton.gameObject.SetActive(_card.CardSave.IsAvailable);
        _card.Activate(_isMoveable);

        SetHierarchy(card.transform);
        Transformation(card.transform);
        OnCoinsChanged(0);

        card.transform.localScale = Vector3.one;

        if (isNew)
            CardOpened?.Invoke(_card);
    }

    private void OnBuyClick()
    {
        if (_card == null)
            return;

        if (_card.CardSave.Amount >= _card.CardSave.AmountToImprove)
            return;

        if (_wallet.TrySpend(_cost))
        {
            _card.CardSave.Add(1);
            _card.Save();

            OnCoinsChanged(0);

            CardBought?.Invoke(_card);
        }
    }

    private void OnCoinsChanged(int amount)
    {
        bool interactable = _card != null && _wallet.Coins >= _cost && _card.CardSave.Amount < _card.CardSave.AmountToImprove;

        _buyButtonImage.color = interactable ? _defaultColor : _inactiveColor;
        _buyButton.interactable = interactable;
    }

    private void SetHierarchy(Transform card)
    {
        card.SetParent(transform);
        card.SetAsLastSibling();
        _buyButton.transform.SetAsLastSibling();
    }

    private void Transformation(Transform card)
    {
        card.localScale = _initialScale;
        card.rotation = Quaternion.identity;
        card.localPosition = Vector3.zero;
    }
}
