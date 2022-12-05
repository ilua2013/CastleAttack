using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TaskStageUI : MonoBehaviour
{
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private Task[] _tasks;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;

    private EnemySpawner _enemySpawner;
    private int _maxCount;

    private void OnValidate()
    {
        _battleSystem = FindObjectOfType<BattleSystem>();
    }

    private void Start()
    {
        ChangeEnemySpawner();
    }

    private void OnEnable()
    {
        _battleSystem.EnemySpawnerChanged += ChangeEnemySpawner;
    }

    private void OnDisable()
    {
        _battleSystem.EnemySpawnerChanged -= ChangeEnemySpawner;
    }

    private void ChangeEnemySpawner()
    {
        if (_enemySpawner != null)
            _enemySpawner.DiedBuild -= UpdateView;

        _enemySpawner = _battleSystem.EnemySpawner;

        if (_enemySpawner.GetComponentInParent<Stage>().StageNumber == StageNumber.One || _enemySpawner.GetComponentInParent<Stage>().StageNumber == StageNumber.Two)
            _maxCount = _enemySpawner.GetBuildCount();
        else if (_enemySpawner.GetComponentInParent<Stage>().StageNumber == StageNumber.Three)
            _maxCount = _enemySpawner.GetBossCount();

        UpdateView();

        _enemySpawner.DiedTarget += UpdateView;
    }

    private void UpdateView()
    {
        foreach (var item in _tasks)
        {
            if(_enemySpawner.GetComponentInParent<Stage>().StageNumber == item.StageNumber)
            {
                _image.sprite = item.Sprite;

                if (item.StageNumber == StageNumber.One || item.StageNumber == StageNumber.Two)
                    _text.text = _maxCount - _enemySpawner.GetBuildCount() + "/" + _maxCount;
                else if (item.StageNumber == StageNumber.Three)
                    _text.text = 1 - _enemySpawner.GetBossCount() + "/" + _maxCount;
            }
        }
    }

    [Serializable]
    private class Task
    {
        public StageNumber StageNumber;
        public FighterType FighterType;
        public Sprite Sprite;
    }
}
