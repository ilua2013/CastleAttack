using System;
using UnityEngine;
using UnityEngine.UI;

public class CardInShopView : MonoBehaviour
{
    private readonly Vector3 _initialScale = new Vector3(2f, 2f, 2f);

    [Header("Content")]
    [SerializeField] private Button _buyButton;
    [SerializeField] private Transform _amountBar;
    [SerializeField] private OpenCardButton _openCardButton;
    [SerializeField] private Image _buyButtonImage;
    [Header("View")]
    [SerializeField] private Color _inactiveColor;
    [SerializeField] private bool _isMoveable;
    [SerializeField] private float _yOffset;
    [Header("Info")]
    [SerializeField] private CardCost _costs;
    [SerializeField] private CardName _cardName;

    private int _cost;
    private Color _defaultColor;
    private Card _card;
    private CoinsWallet _wallet;

    public Card Card => _card;
    public int Cost => _cost;
    public CardName CardName => _cardName;

    public event Action<Card> CardBought;
    public event Action<Card> CardOpened;
    public event Action<int> CostUpdated;
    public event Action<bool> CardFull;
    public event Action<Card> Inited;

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
        _card.Load();

        _cost = _costs.GetCost(_card.CardSave);

        _card.gameObject.SetActive(true);

        if (IsNotUnit())
            _buyButton.gameObject.SetActive(_card.CardSave.IsAvailable);

        _openCardButton.enabled = !_card.CardSave.IsAvailable;
        _openCardButton.transform.SetAsLastSibling();

        _card.Activate(_isMoveable);

        SetHierarchy(card.transform);
        Transformation(card.transform);
        OnCoinsChanged(0, 0);

        card.transform.localScale = Vector3.one;
        card.transform.localScale = Vector3.one * .85f;

        if (isNew)
            CardOpened?.Invoke(_card);

        CostUpdated?.Invoke(_cost);
        CardFull?.Invoke(_card.CardSave.CanLevelUp);
        Inited?.Invoke(_card);
    }

    private void OnBuyClick()
    {
        if (_card == null)
            return;

        if (_card.CardSave.Amount >= _card.CardSave.AmountToImprove)
            return;

        if (_wallet.TrySpend(_costs.GetCost(_card.CardSave)))
        {
            _card.CardSave.Add(1);
            _card.Save();

            if (!_card.CardSave.CanLevelUp)
            {
                _cost = _costs.GetCost(_card.CardSave);
                CostUpdated?.Invoke(_cost);
            }
            else
            {
                CardFull?.Invoke(true);
            }

            OnCoinsChanged(0, 0);
            CardBought?.Invoke(_card);
        }
    }

    private void OnCoinsChanged(int amount, float delay)
    {
        bool interactable = CanBuy();

        _buyButtonImage.color = interactable ? _defaultColor : _inactiveColor;
        _buyButton.interactable = interactable;

        _cost = _costs.GetCost(_card.CardSave);
        CostUpdated?.Invoke(_cost);
    }

    private void SetHierarchy(Transform card)
    {
        card.SetParent(transform);
    }

    private void Transformation(Transform card)
    {
        card.localScale = _initialScale;
        card.rotation = Quaternion.identity;
        card.localPosition = new Vector3(0, _yOffset, 0);
    }

    private bool CanBuy()
    {
        return _card != null && _wallet.Coins >= _costs.GetCost(_card.CardSave) && _card.CardSave.Amount < _card.CardSave.AmountToImprove;
    }

    private bool IsNotUnit()
    {
        return CardName != CardName.Hand && CardName != CardName.Ork && CardName != CardName.Snake && CardName != CardName.Bat;
    }
}
