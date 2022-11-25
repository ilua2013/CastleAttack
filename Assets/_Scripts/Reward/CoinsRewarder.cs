using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsRewarder : MonoBehaviour
{
    private LevelSystem _levelSystem;

    private void Awake()
    {
        _levelSystem = FindObjectOfType<LevelSystem>();
    }

    private void OnEnable()
    {
        _levelSystem.Wave3Finished += OnFinished;
    }

    private void OnDisable()
    {
        _levelSystem.Wave3Finished -= OnFinished;
    }

    private void OnFinished()
    {

    }
}
