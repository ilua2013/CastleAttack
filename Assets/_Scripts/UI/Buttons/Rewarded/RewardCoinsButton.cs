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
#if UNITY_EDITOR
        OnRewardedCallback();
#else
        YandexSDK.Instance.ShowVideoAd(OnRewardedCallback);
#endif
    }

    private void OnRewardedCallback()
    {
        _wallet.Add(_award);
#if !UNITY_EDITOR
        YandexMetrica.Send("MoneyAdClick");
#endif
    }
}
