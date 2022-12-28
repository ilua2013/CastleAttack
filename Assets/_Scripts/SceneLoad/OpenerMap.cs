using Agava.YandexMetrica;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenerMap : MonoBehaviour
{
    [SerializeField] private MapLevels _map;
    [SerializeField] private bool _showAD = true;

    public Button Button { get; private set; }

    public event Action NextLevelLoaded;

    private void OnValidate()
    {
        _map = FindObjectOfType<MapLevels>();
    }

    private void Awake()
    {
        Button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        Button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        NextLevelLoaded?.Invoke();

        _map.OpenMap();
    }
}
