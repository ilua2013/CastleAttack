using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Agava.YandexGames;

public class InitLoaderScene : MonoBehaviour
{
    private const int TutorialIndex = 2;
    private const int MenuIndex = 1;

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
        //int index = TutorialIndex;
        int index = MenuIndex;

        if (Saves.HasKey(SaveController.Params.IsTutorialCompleted))
            if (Saves.GetBool(SaveController.Params.IsTutorialCompleted))
                index = MenuIndex;

        _sceneLoader.LoadScene(index);
    }
}