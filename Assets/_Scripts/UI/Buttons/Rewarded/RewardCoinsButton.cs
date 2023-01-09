using Agava.YandexMetrica;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RewardCoinsButton : MonoBehaviour
{
    [SerializeField] private CoinsWallet _wallet;
    [SerializeField] private LevelRewardData _levelRewardData;
    [SerializeField] private TMP_Text _text;

    private int _award;
    private Button _button;

    private void OnValidate()
    {
        if (_wallet == null)
            _wallet = FindObjectOfType<CoinsWallet>();

        if (_levelRewardData == null)
            _levelRewardData = Resources.Load("Configs/LevelReward") as LevelRewardData;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (Saves.HasKey(SaveController.Params.Level))
            _award = _levelRewardData.GetAward(Saves.GetInt(SaveController.Params.Level) - 2).Coins;
        else
            _award = 100;

        _text.text = _award.ToString();
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
        _wallet.Add(_award, 0);
#if !UNITY_EDITOR
        YandexMetrica.Send("MoneyAdClick");
#endif
    }
}
