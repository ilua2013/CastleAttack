using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;

public class CoinsWalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _delayAnimation = 3.7f;

    private Animator _animator;
    private CoinsWallet _coinsWallet;
    private float _current;

    private const string Reward = "Reward";

    private void Awake()
    {
        _coinsWallet = FindObjectOfType<CoinsWallet>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _coinsWallet.CoinsChanged += OnRewarded;
    }

    private void OnDisable()
    {
        _coinsWallet.CoinsChanged -= OnRewarded;
    }

    private void Start()
    {
        _current = _coinsWallet.Coins;
        _text.text = FormatCost(_current);
    }

    private void OnRewarded(int amount)
    {
        _current += amount;
        StartCoroutine(Adding(amount));
    }

    private IEnumerator Adding(int amount, Action onComplete = null)
    {
        float coins = _current - amount;
        float time = 0;
        yield return new WaitForSeconds(_delayAnimation);
        _animator.SetTrigger(Reward);

        while (coins != _current)
        {
            coins = Mathf.MoveTowards(coins, _current, 0.35f);
            _text.text = FormatCost(coins);
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _text.text = FormatCost(_current);
        onComplete?.Invoke();
    }

    private string FormatCost(float cost)
    {
        var format = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        format.NumberGroupSeparator = " ";
        format.NumberDecimalDigits = 0;

        return Convert.ToDecimal(cost).ToString("N", format);
    }
}
