using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
public class CardLevelUpButton : MonoBehaviour
{
    private const int Cost = 100;

    [SerializeField] private CardInDeckView _cardView;

    private Button _button;
    private CoinsWallet _coinsWallet;

    public event Action LevelUp;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _coinsWallet = FindObjectOfType<CoinsWallet>();
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
        if (_coinsWallet.TrySpend(Cost))
        {
            if (_cardView.TryLevelUpCard())
                LevelUp?.Invoke();
        }
    }
}
