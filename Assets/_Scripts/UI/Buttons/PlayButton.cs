using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PlayButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private string NextScene;

    private Button _button;
    private LevelLoader _levelLoader;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _levelLoader = new LevelLoader();
    }

    private void OnEnable() => _button.onClick.AddListener(OnClick);

    private void OnDisable() => _button.onClick.RemoveListener(OnClick);

    private void OnClick()
    {
        AsyncOperation waitLoadScene = _levelLoader.LoadScene(NextScene);
        StartCoroutine(LoadingCurtain(waitLoadScene));
    }

    private IEnumerator LoadingCurtain(AsyncOperation waitLoadScene)
    {
        float lerpSpeed = 1f;

        while (!waitLoadScene.isDone)
        {
            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1 - waitLoadScene.progress, lerpSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
