using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour
{
    private const int TutorialIndex = 2;
    private const int MenuIndex = 1;

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
        int index = TutorialIndex;

        if (Saves.HasKey(SaveController.Params.IsTutorialCompleted))
            if (Saves.GetBool(SaveController.Params.IsTutorialCompleted))
                index = MenuIndex;

        SceneManager.LoadScene(index);
    }
}
