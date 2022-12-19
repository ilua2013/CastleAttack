using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WizardStatsView : MonoBehaviour
{
    private const int DamageMax = 10;
    private const int HealthMax = 10;

    private const int DamageCost = 100;
    private const int HealthCost = 100;

    [SerializeField] private Button _healthButton;
    [SerializeField] private Button _damageButton;

    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _damageText;

    [SerializeField] private WizardStats _wizardStats;
    [SerializeField] private CoinsWallet _wallet;

    private void OnValidate()
    {
        if (_wizardStats == null)
            _wizardStats = FindObjectOfType<WizardStats>();

        if (_wallet == null)
            _wallet = FindObjectOfType<CoinsWallet>();
    }

    private void OnEnable()
    {
        _wizardStats.Loaded += OnLoaded;
        _wallet.CoinsChanged += OnCoinsChanged;

        _healthButton.onClick.AddListener(OnClickHealth);
        _damageButton.onClick.AddListener(OnClickDamage);
    }

    private void OnDisable()
    {
        _wizardStats.Loaded -= OnLoaded;
        _wallet.CoinsChanged -= OnCoinsChanged;

        _healthButton.onClick.AddListener(OnClickHealth);
        _damageButton.onClick.AddListener(OnClickDamage);
    }

    private void Start()
    {
        OnLoaded(_wizardStats.Stats);
    }

    private void OnCoinsChanged(int amount, float delay)
    {
        OnLoaded(_wizardStats.Stats);
    }

    private void OnClickDamage()
    {
        if (_wallet.TrySpend(DamageCost))
            _wizardStats.AddDamage(1);
    }

    private void OnClickHealth()
    {
        if (_wallet.TrySpend(HealthCost))
            _wizardStats.AddMaxHealth(1);
    }

    private void OnLoaded(Stats stats)
    {
        _healthText.text = stats.MaxHealth.ToString();
        _damageText.text = stats.Damage.ToString();

        _healthButton.interactable = _wallet.HaveCoins(HealthCost);
        _damageButton.interactable = _wallet.HaveCoins(DamageCost);
    }
}
