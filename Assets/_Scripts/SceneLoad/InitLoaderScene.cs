using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Agava.YandexGames;

public class InitLoaderScene : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;

    private void OnValidate()
    {
        _sceneLoader = FindObjectOfType<SceneLoader>();
    }

    private void OnEnable()
    {
        YandexSDK.Instance.Initialized += OnInitialized;
    }

    private void OnDisable()
    {
        YandexSDK.Instance.Initialized -= OnInitialized;
    }

    private void OnInitialized()
    {
        _sceneLoader.LoadNextLevel();
    }
}
