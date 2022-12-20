using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    public const int OpeningIndex = 1;
    public const int TutorialIndex = 2;
    public const int MenuIndex = 3;
    public const int FirstLevelIndex = 4;

    [SerializeField] private Image _background;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TextMeshProUGUI _loadingPercent;

    private AsyncOperation _load;

    private void OnValidate()
    {
        _background.enabled = false;
        _progressSlider.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        LoadScene(MenuIndex);
    }

    public void LoadNextLevel()
    {
        int levelIndex = OpeningIndex;

        if (Saves.HasKey(SaveController.Params.IsOpeningViewed))
            if (Saves.GetBool(SaveController.Params.IsOpeningViewed))
                levelIndex = TutorialIndex;

        if (Saves.HasKey(SaveController.Params.IsTutorialCompleted))
            if (Saves.GetBool(SaveController.Params.IsTutorialCompleted))
                levelIndex = FirstLevelIndex;

        if (Saves.HasKey(SaveController.Params.Level))
            if (Saves.GetInt(SaveController.Params.Level) + 1 < SceneManager.sceneCountInBuildSettings)
                levelIndex = Saves.GetInt(SaveController.Params.Level) + 1;

        LoadScene(levelIndex);
    }

    private void LoadScene(int index)
    {
        _background.enabled = true;
        _progressSlider.gameObject.SetActive(true);

        StartCoroutine(AsyncLoad(index));
    }

    private IEnumerator AsyncLoad(int sceneIndex)
    {
        _load = SceneManager.LoadSceneAsync(sceneIndex);

        _load.allowSceneActivation = false;

        while (_progressSlider.value < 1)
        {
            _progressSlider.value += Time.timeScale * 0.05f;
            _loadingPercent.text = Mathf.RoundToInt(_progressSlider.value * 100) + "%";
            yield return null;
        }

        _load.allowSceneActivation = true;
    }
}
