using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;
using System;

public class CoinsWalletView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    
    private CoinsWallet _coinsWallet;

    private void Awake()
    {
        _coinsWallet = FindObjectOfType<CoinsWallet>();
    }

    private void Update()
    {
        _text.text = FormatCost(_coinsWallet.Coins);
    }

    private string FormatCost(float cost)
    {
        var format = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        format.NumberGroupSeparator = " ";
        format.NumberDecimalDigits = 0;

        return Convert.ToDecimal(cost).ToString("N", format);
    }
}
