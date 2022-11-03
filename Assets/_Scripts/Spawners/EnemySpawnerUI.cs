using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawnerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private EnemySpawner _enemySpawner;

    private void Start()
    {
        SetText();
    }

    private void OnEnable()
    {
        _enemySpawner.WaveCountChanged += SetText;
    }

    private void OnDisable()
    {
        _enemySpawner.WaveCountChanged -= SetText;
    }

    private void SetText()
    {
        _text.text = GetWaveCount();
    }

    private string GetWaveCount()
    {
        return "Wave " + _enemySpawner.CurrentWave + "/" + _enemySpawner.WaveCount;
    }
}
