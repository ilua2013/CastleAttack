using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WizardStats : MonoBehaviour
{
    [SerializeField] private Stats _stats;
    [SerializeField] private UnitFriend _unitFriend;

    public Stats Stats => _stats;

    public event Action<Stats> Loaded;
    
    private void OnValidate()
    {
        if (TryGetComponent(out UnitFriend unitFriend))
            _unitFriend = unitFriend;
    }

    private void Awake()
    {
        _stats.Load();

        if (_unitFriend != null)
        {
            _unitFriend.Fighter.SetHealth(_stats.MaxHealth);
            _unitFriend.Fighter.SetDamage(_stats.Damage);
        }
    }

    private void Start()
    {
        Loaded?.Invoke(_stats);
    }

    public void AddMaxHealth(int amount)
    {
        _stats.AddMaxHealth(amount);

        Loaded?.Invoke(_stats);
    }

    public void AddDamage(int amount)
    {
        _stats.AddDamage(amount);

        Loaded?.Invoke(_stats);
    }
}

[Serializable]
public class Stats
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;

    public int MaxHealth => _maxHealth;
    public int Damage => _damage;

    public void Load()
    {
        if (Saves.HasKey(SaveController.Params.WizardStats))
        {
            Stats stats = Saves.GetWizardStats(SaveController.Params.WizardStats);

            _maxHealth = stats.MaxHealth;
            _damage = stats.Damage;
        }
    }

    public void AddMaxHealth(int amount)
    {
        _maxHealth += amount;

        Saves.SetWizardStats(SaveController.Params.WizardStats, this);
        Saves.Save();
    }

    public void AddDamage(int amount)
    {
        _damage += amount;

        Saves.SetWizardStats(SaveController.Params.WizardStats, this);
        Saves.Save();
    }
}
