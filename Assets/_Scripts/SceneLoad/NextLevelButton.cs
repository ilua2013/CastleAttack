using Agava.YandexMetrica;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextLevelButton : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;

    public Button Button { get; private set; }

    public event Action NextLevelLoaded;

    private void OnValidate()
    {
        _sceneLoader = FindObjectOfType<SceneLoader>();
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

#if !UNITY_EDITOR
        YandexSDK.Instance.ShowInterstitial(null, () => _sceneLoader.LoadNextLevel());
#else
        _sceneLoader.LoadNextLevel();
#endif
    }
}
