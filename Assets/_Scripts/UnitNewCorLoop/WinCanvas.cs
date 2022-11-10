using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCanvas : MonoBehaviour
{
    [SerializeField] private LevelSystem _levelSystem;
    [SerializeField] private GameObject _winCanvas;
    [SerializeField] private GameObject _loseCanvas;

    private void Start()
    {
        _winCanvas.SetActive(false);
        _loseCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        _levelSystem.Failed += Lose;
        _levelSystem.Wave3Finished += Win;
    }
    private void OnDisable()
    {
        _levelSystem.Failed -= Lose;
        _levelSystem.Wave3Finished -= Win;
    }

    private void Win()
    {
        _winCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    private void Lose()
    {
        _loseCanvas.SetActive(true);
        Time.timeScale = 0;
    }
}
