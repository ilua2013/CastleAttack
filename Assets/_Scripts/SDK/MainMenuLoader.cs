using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    [SerializeField] private string _sceneName;

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
        SceneManager.LoadScene(_sceneName);
    }
}
