using Agava.YandexMetrica;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RewardCoinsButton : MonoBehaviour
{
    [SerializeField] private int _award;
    [SerializeField] private CoinsWallet _wallet;

    private Button _button;

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

    private void OnClick()
    {
        YandexSDK.Instance.ShowVideoAd(OnRewardedCallback);
    }

    private void OnRewardedCallback()
    {
        _wallet.Add(_award);
        YandexMetrica.Send("moneyAdClick");
    }
}
