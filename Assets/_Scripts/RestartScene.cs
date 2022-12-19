using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    [SerializeField] private Button _buttonLoadScene;
    [SerializeField] private SceneLoader _sceneLoader;

    private void OnValidate()
    {
        _sceneLoader = FindObjectOfType<SceneLoader>();

        if(_buttonLoadScene == null && TryGetComponent(out Button button))
            _buttonLoadScene = button;
    }

    private void OnEnable()
    {
        _buttonLoadScene.onClick.AddListener(LoadMenu);
    }

    private void OnDisable()
    {
        _buttonLoadScene.onClick.RemoveListener(LoadMenu);
    }

    private void LoadMenu()
    {
#if !UNITY_EDITOR
        YandexSDK.Instance.ShowInterstitial(null, () =>
        {
            _sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
#else
        _sceneLoader.LoadScene(SceneManager.GetActiveScene().buildIndex);
#endif
    }
}
