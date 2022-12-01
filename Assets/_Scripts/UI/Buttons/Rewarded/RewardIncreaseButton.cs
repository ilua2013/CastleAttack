using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RewardIncreaseButton : MonoBehaviour
{
    [SerializeField] private int _factor;

    private Button _button;
    private Card[] _rewardCards;

    public event Action<int> Rewarded;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    public void Init(Card[] rewardCards)
    {
        _rewardCards = rewardCards;
    }

    private void OnClick()
    {
        YandexSDK.Instance.ShowVideoAd(OnRewardedCallback);
    }

    private void OnRewardedCallback()
    {
        foreach (Card card in _rewardCards)
            card.CardSave.Add(card.CardSave.Amount * _factor);

        Rewarded?.Invoke(_factor);
        gameObject.SetActive(false);
    }
}
