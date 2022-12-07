using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCardButton : MonoBehaviour
{
    [SerializeField] private Button _costButton;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private int _cost;

    private CoinsWallet _wallet;
    private DeckBuilder _deck;
    private CardInShopView _view;

    private void Awake()
    {
        _wallet = FindObjectOfType<CoinsWallet>();
        _deck = FindObjectOfType<DeckBuilder>();
        _view = GetComponentInParent<CardInShopView>();
    }

    private void OnEnable()
    {
        _costButton.onClick.AddListener(OnCostClick);
        _rewardButton.onClick.AddListener(OnRewardedClick);

        if (_view.Card != null && _view.Card.CardSave.IsAvailable)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _costButton.onClick.RemoveListener(OnCostClick);
        _rewardButton.onClick.RemoveListener(OnRewardedClick);
    }

    private void Start()
    {
        if (_view.Card != null && _view.Card.CardSave.IsAvailable)
            gameObject.SetActive(false);
    }

    private void OnCostClick()
    {
        if (_wallet.TrySpend(_cost))
            OpenCard();
    }

    private void OnRewardedClick()
    {
#if UNITY_EDITOR
        OpenCard();
        return;
#endif
        YandexSDK.Instance.ShowVideoAd(OpenCard);
    }

    private void OpenCard()
    {
        Card card = Instantiate(_deck.TakeCard(_view.CardName));

        card.gameObject.SetActive(true);
        card.CardSave.SetAvailable(true);
        card.Save(card.CardSave);

        _view.FillCard(card, true);
        gameObject.SetActive(false);
    }
}
