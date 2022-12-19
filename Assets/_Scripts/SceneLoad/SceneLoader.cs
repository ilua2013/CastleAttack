using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TextMeshProUGUI _loadingPercent;

    private AsyncOperation _load;

    private void OnValidate()
    {
        _background.enabled = false;
        _progressSlider.gameObject.SetActive(false);
    }

    public void LoadScene(int sceneIndex)
    {
        _background.enabled = true;
        _progressSlider.gameObject.SetActive(true);

        StartCoroutine(AsyncLoad(sceneIndex));
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
