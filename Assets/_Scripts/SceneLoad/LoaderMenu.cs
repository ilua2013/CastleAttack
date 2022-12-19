using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoaderMenu : MonoBehaviour
{
    [SerializeField] private int _indexMenu = 1;
    [SerializeField] private Button _buttonLoadScene;
    [SerializeField] private SceneLoader _sceneLoader;

    private void OnValidate()
    {
        _sceneLoader = FindObjectOfType<SceneLoader>();

        if (_buttonLoadScene == null && TryGetComponent(out Button button))
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
        _sceneLoader.LoadScene(_indexMenu);
    }
}
