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

        _maxCount = _enemySpawner.GetBuildCount();
        UpdateView();

        _enemySpawner.DiedBuild += UpdateView;
    }

    private void UpdateView()
    {
        print("UpdateView");
        foreach (var item in _tasks)
        {
            if(_enemySpawner.GetComponentInParent<Stage>().StageNumber == item.StageNumber)
            {
                _image.sprite = item.Sprite;
                _text.text = _maxCount - _enemySpawner.GetBuildCount() + "/" + _maxCount;
                print(_enemySpawner.GetBuildCount() + " count Build");
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
