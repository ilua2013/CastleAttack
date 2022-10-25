using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelSystem : MonoBehaviour
{
    [SerializeField] private Button _buttonStartFight;

    public event Action ClickedStartFight;
    public event Action FightFinished; // надо чтоб это как-то вызывалось

    private void OnEnable()
    {
        _buttonStartFight.onClick.AddListener(StartFight);
    }

    private void OnDisable()
    {
        _buttonStartFight.onClick.RemoveListener(StartFight);
    }

    private void StartFight()
    {
        ClickedStartFight?.Invoke();
    }
}
