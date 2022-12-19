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
    private const int TutorialIndex = 2;
    private const int FirstLevelIndex = 3;

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

        int levelIndex = FirstLevelIndex;

        //        if (Saves.HasKey(SaveController.Params.IsTutorialCompleted)) // –¿«¡ÀŒ »–Œ¬¿“‹  Œ√ƒ¿ “”“Œ–»¿À ¡”ƒ≈“ √Œ“Œ¬
        //        {
        //            if (Saves.GetBool(SaveController.Params.IsTutorialCompleted) == false)
        //            {
        //#if !UNITY_EDITOR
        //                YandexMetrica.Send("LevelComplete", new Dictionary<string, string>() { { "Level", "Tutorial" } });
        //#endif
        //                _sceneLoader.LoadScene(TutorialIndex);
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            _sceneLoader.LoadScene(TutorialIndex);
        //#if !UNITY_EDITOR
        //            YandexMetrica.Send("LevelComplete", new Dictionary<string, string>() { { "Level", "Tutorial" } });
        //#endif
        //            return;
        //        }

        if (Saves.HasKey(SaveController.Params.Level))
            levelIndex = Saves.GetInt(SaveController.Params.Level) + 1;

        if (levelIndex >= SceneManager.sceneCountInBuildSettings)
            levelIndex = FirstLevelIndex;

#if !UNITY_EDITOR
        YandexSDK.Instance.ShowInterstitial(null, () => _sceneLoader.LoadScene(levelIndex));
        YandexMetrica.Send("LevelComplete", new Dictionary<string, string>() { { "Level", $"{levelIndex - 2}" } });
#else
        _sceneLoader.LoadScene(levelIndex);
#endif
    }
}
