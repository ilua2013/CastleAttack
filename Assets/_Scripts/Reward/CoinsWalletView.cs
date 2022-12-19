using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System;

public class CoinsWalletView : MonoBehaviour
{
    private const string Reward = "Reward";

    [SerializeField] private TMP_Text _text;
    [SerializeField] private CoinsWallet _coinsWallet;

    private Animator _animator;
    private float _current;

    private void OnValidate()
    {
        _coinsWallet = FindObjectOfType<CoinsWallet>(true);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        _current = _coinsWallet.Coins;
        _text.text = FormatCost(_current);
    }

    public void OnSpent(int amount)
    {
        _current -= amount;

        if (gameObject.activeInHierarchy)
            StartCoroutine(Spent(amount));
    }

    public void OnRewarded(int amount, float delay)
    {
        _current += amount;

        if (gameObject.activeInHierarchy)
            StartCoroutine(Adding(amount, delay));
    }

    private IEnumerator Spent(int amount)
    {
        float coins = _current + amount;

        while (coins != _current)
        {
            coins = Mathf.MoveTowards(coins, _current, 0.75f);
            _text.text = FormatCost(coins);

            yield return new WaitForEndOfFrame();
        }

        _text.text = FormatCost(_current);
    }

    private IEnumerator Adding(int amount, float delay, Action onComplete = null)
    {
        float coins = _current - amount;
        float time = 0;

        yield return new WaitForSeconds(delay);

        _animator.SetTrigger(Reward);

        while (coins != _current)
        {
            coins = Mathf.MoveTowards(coins, _current, 0.45f);
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
