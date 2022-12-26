using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCardButton : MonoBehaviour
{
    [SerializeField] private Button _costButton;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private int _cost;
    [SerializeField] private CardDimmer _cardDimmer;
    [SerializeField] private ParticleSystem _vfx;

    private CoinsWallet _wallet;
    private DeckBuilder _deck;
    private CardInShopView _view;

    public bool _isParticleActive;

    private void Awake()
    {
        _wallet = FindObjectOfType<CoinsWallet>();
        _deck = FindObjectOfType<DeckBuilder>();
        _view = GetComponentInParent<CardInShopView>();
    }

    private void Update()
    {
        if (_isParticleActive)
        {
            _vfx.Play();
            _isParticleActive = false;
        }

        Debug.Log(_vfx.isPlaying);
    }

    private void OnEnable()
    {
        if (_view.Card != null && _view.Card.CardSave.IsAvailable)
            gameObject.SetActive(false);

        _view.Inited += OnInitied;
        _costButton.onClick.AddListener(OnCostClick);
        _rewardButton.onClick.AddListener(OnRewardedClick);
    }

    private void OnDisable()
    {
        _view.Inited -= OnInitied;
        _costButton.onClick.RemoveListener(OnCostClick);
        _rewardButton.onClick.RemoveListener(OnRewardedClick);

        _cardDimmer.Inactivate();
    }

    private void Start()
    {
        if (_view.Card != null && _view.Card.CardSave.IsAvailable)
            gameObject.SetActive(false);
    }
    
    private void OnInitied(Card card)
    {
        _cardDimmer.Init(_view.Card);
        _cardDimmer.Activate();
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
        _view.Card.gameObject.SetActive(true);
        _view.Card.CardSave.SetAvailable(true);
        _view.Card.Save(_view.Card.CardSave);

        _view.FillCard(_view.Card, true);

        _vfx.transform.SetParent(transform.parent);
        _vfx.transform.SetAsLastSibling();
        _vfx.Play();

        //gameObject.SetActive(false);
    }
}
