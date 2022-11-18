using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GateType
{
    First,
    Second
}

public class GateOpenAnimation : MonoBehaviour
{
    [SerializeField] private GateType _gateType;

    private const string OpenTrigger = "Open";

    private Animator _animator;
    private LevelSystem _levelSystem;

    private void Awake()
    {
        _levelSystem = FindObjectOfType<LevelSystem>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _levelSystem.Wave1Finished += OnWaveFinish;
        _levelSystem.Wave2Finished += OnWaveFinish;
        _levelSystem.Wave3Finished += OnWaveFinish;
    }

    private void OnDisable()
    {
        _levelSystem.Wave1Finished -= OnWaveFinish;
        _levelSystem.Wave2Finished -= OnWaveFinish;
        _levelSystem.Wave3Finished -= OnWaveFinish;
    }

    private void OnWaveFinish()
    {
        //if (_gateType == GateType.First)
        //    _animator.SetTrigger(OpenTrigger);
    }
}
