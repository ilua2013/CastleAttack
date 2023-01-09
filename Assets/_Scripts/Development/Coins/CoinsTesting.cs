using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsTesting : MonoBehaviour
{
    [SerializeField] private int _amount = 50;
    [SerializeField] private Button _plus;
    [SerializeField] private Button _minus;

    [SerializeField] private CoinsWallet _coinsWallet;

    private void OnValidate()
    {
        if (_coinsWallet == null)
            _coinsWallet = FindObjectOfType<CoinsWallet>();
    }

    private void OnEnable()
    {
        _plus.onClick.AddListener(OnPlus);
        _minus.onClick.AddListener(OnMinus);
    }

    private void OnDisable()
    {
        _plus.onClick.RemoveListener(OnPlus);
        _minus.onClick.RemoveListener(OnMinus);
    }

    private void OnPlus()
    {
        _coinsWallet.Add(_amount, 0);
    }

    private void OnMinus()
    {
        _coinsWallet.TrySpend(_amount);
    }
}
