using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitFriend))]
public class UnitExperience : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfx;
    [SerializeField] private int _threshold = 2;
    [SerializeField] private int _thresholdPlusUp = 1;
    [SerializeField] private int _level;

    private UnitFriend _unit;
    private int _current;

    public int Thresold => _threshold;
    public int CurrentKill => _current;
    public int Level => _level;

    public event Action<int> Up;
    public event Action<int> LevelUp;

    private void Awake()
    {
        _unit = GetComponent<UnitFriend>();
    }

    private void OnEnable()
    {
        _unit.EnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        _unit.EnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(UnitEnemy enemy)
    {
        if (_level >= 3)
            return;

        _current++;
        Up?.Invoke(_current);

        if (_current >= _threshold)
        {
            _level++;
            _current = 0;
            _threshold += _thresholdPlusUp;

            CreateNextLevel();
        }
    }

    public void CreateNextLevel()
    {
        StartCoroutine(DestroyWithDelay(2f));
        StartCoroutine(_unit.DestroyWithDelay(2f));
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        LevelUp?.Invoke(_level);
        _vfx.transform.SetParent(null);
        _vfx.Play();
        _unit.Card.DoStageUp(_unit);
    }
}
