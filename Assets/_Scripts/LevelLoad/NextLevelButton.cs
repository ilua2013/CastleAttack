using Agava.YandexMetrica;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class NextLevelButton : MonoBehaviour
{
    private const int TutorialIndex = 2;
    private const int FirstLevelIndex = 3;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        int levelIndex = FirstLevelIndex;

        if (Saves.HasKey(SaveController.Params.IsTutorialCompleted))
        {
            if (Saves.GetBool(SaveController.Params.IsTutorialCompleted) == false)
            {
#if !UNITY_EDITOR
                YandexSDK.Instance.ShowInterstitial();
                YandexMetrica.Send("LevelComplete", new Dictionary<string, string>() { { "Level", "Tutorial" } });
#endif
                SceneManager.LoadScene(TutorialIndex);
                return;
            }
        }
        else
        {
            SceneManager.LoadScene(TutorialIndex);
#if !UNITY_EDITOR
            YandexMetrica.Send("LevelComplete", new Dictionary<string, string>() { { "Level", "Tutorial" } });
#endif
            return;
        }

        if (Saves.HasKey(SaveController.Params.Level))
            levelIndex = Saves.GetInt(SaveController.Params.Level) + 1;

        if (levelIndex >= SceneManager.sceneCountInBuildSettings)
            levelIndex = FirstLevelIndex;

#if !UNITY_EDITOR
        YandexSDK.Instance.ShowInterstitial();
        YandexMetrica.Send("LevelComplete", new Dictionary<string, string>() { { "Level", $"{levelIndex - 2}" } });
#endif

        SceneManager.LoadScene(levelIndex);
    }
}
