using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCardButton : MonoBehaviour
{
    private const string ChainDestroyState = "ChainDestroy";
    private const string ShowState = "Show";

    [SerializeField] private Button _costButton;
    [SerializeField] private Button _rewardButton;
    [SerializeField] private CardDimmer _cardDimmer;
    [SerializeField] private int _cost;
    [Header("VFX")]
    [SerializeField] private ParticleSystem _vfx;
    [Header("Animators")]
    [SerializeField] private Animator _buyButtonAnimator;
    [SerializeField] private Animator _amountBarAnimator;
    [SerializeField] private Animator _chainAnimator;

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
        if (_view.Card != null && _view.Card.CardSave.IsAvailable)
            gameObject.SetActive(false);

        _view.Inited += OnInitied;
        _costButton.onClick.AddListener(OnCostClick);
        _rewardButton.onClick.AddListener(OnRewardedClick);

        _chainAnimator.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _view.Inited -= OnInitied;
        _costButton.onClick.RemoveListener(OnCostClick);
        _rewardButton.onClick.RemoveListener(OnRewardedClick);

        Inactivate();

        _chainAnimator.gameObject.SetActive(false);
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
        Card card = _view.Card;

        AddCard(card);
        
        _chainAnimator.Play(ChainDestroyState);

        StartCoroutine(PlayVFX(_vfx, 0.45f));
        StartCoroutine(HoverCard(card.transform, 0.5f, () => _view.FillCard(card, true)));
        Inactivate();
    }

    private void Inactivate()
    {
        _costButton.gameObject.SetActive(false);
        _rewardButton.gameObject.SetActive(false);

        _cardDimmer.Inactivate();
    }

    private void AddCard(Card card)
    {
        card.gameObject.SetActive(true);
        card.CardSave.SetAvailable(true);
        card.CardSave.Add(1);
        card.Save(card.CardSave);
    }

    private IEnumerator PlayVFX(ParticleSystem vfx, float delay)
    {
        yield return new WaitForSeconds(delay);

        ParticleSystem particle = Instantiate(vfx, transform);

        particle.transform.SetParent(transform.parent);
        particle.transform.SetAsLastSibling();
        particle.Play();

        yield return new WaitForSeconds(particle.main.duration);

        Destroy(particle.gameObject);
    }

    private IEnumerator HoverCard(Transform card, float delay, Action onEndCallback = null)
    {
        float distanceDelta = 0.01f;
        float lerpTime = 10f;
        float scale = 1.25f;

        Vector3 to = new Vector3(scale, scale, scale);

        yield return new WaitForSeconds(delay);

        while (Vector3.Distance(card.localScale, to) > distanceDelta)
        {
            card.localScale = Vector3.Lerp(card.localScale, to, lerpTime * Time.deltaTime);
            yield return null;
        }

        card.localScale = to;

        yield return new WaitForSeconds(0.2f);

        while (Vector3.Distance(card.localScale, Vector3.one) > distanceDelta)
        {
            card.localScale = Vector3.Lerp(card.localScale, Vector3.one, lerpTime * Time.deltaTime);
            yield return null;
        }

        card.localScale = Vector3.one;

        onEndCallback?.Invoke();

        _buyButtonAnimator.Play(ShowState);
        _amountBarAnimator.Play(ShowState);
    }
}
