using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemySpawnerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private BattleSystem _battleSystem;

    private EnemySpawner _enemySpawner;
    private string _initialText;

    private void OnValidate()
    {
        _battleSystem = FindObjectOfType<BattleSystem>();
    }

    private void Start()
    {
        _initialText = _text.text;
        SetSpawner();
    }

    private void OnEnable()
    {
        _battleSystem.EnemySpawnerChanged += SetSpawner;
    }

    private void OnDisable()
    {
        _battleSystem.EnemySpawnerChanged -= SetSpawner;
    }

    public void SetSpawner()
    {
        if (_enemySpawner != null)
            _enemySpawner.WaveCountChanged -= SetText;

        _enemySpawner = _battleSystem.EnemySpawner;
        _enemySpawner.WaveCountChanged += SetText;

        SetText();
    }

    private void SetText()
    {
        _text.text = GetWaveCount();
    }

    private string GetWaveCount()
    {
        return $"{_initialText} " + _enemySpawner.CurrentWave + "/" + _enemySpawner.WaveCount;
    }
}
