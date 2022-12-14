using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RewardRespawnButton : MonoBehaviour
{
    private Button _button;

    public event Action Respawned;

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
#if !UNITY_EDITOR
        YandexSDK.Instance.ShowVideoAd(OnRewardedCallback);
        gameObject.SetActive(false);
        return;
#endif
        OnRewardedCallback();
        gameObject.SetActive(false);
    }

    private void OnRewardedCallback()
    {
        Respawned?.Invoke();
    }
}
