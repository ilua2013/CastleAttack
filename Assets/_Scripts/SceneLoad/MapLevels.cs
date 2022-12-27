using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapLevels : MonoBehaviour
{
    [SerializeField] private List<LevelOnMap> _levels;
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private int _firstIndexLevel = 3;
    [Header("Panel Start Level")]
    [SerializeField] private RectTransform _panelStartLevel;
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonClose;
    [SerializeField] private float _speedAnimation;

    private LevelOnMap _currentLevel;

    private void OnValidate()
    {
        _levels = GetComponentsInChildren<LevelOnMap>().ToList();
        _sceneLoader = FindObjectOfType<SceneLoader>();
        _panelStartLevel.localScale = Vector3.zero;

        int indexNumber = _firstIndexLevel;

        foreach (var item in _levels)
        {
            item.IndexScene = indexNumber;
            indexNumber++;
        }
    }

    private void Awake()
    {
        _panelStartLevel.parent = GetComponentInParent<Canvas>().transform;
        _panelStartLevel.localScale = Vector3.zero;
    }

    private void Start()
    {
        InitLevels();
    }

    private void OnEnable()
    {
        foreach (var item in _levels)
            item.Clicked += SelectLevel;

        _buttonPlay.onClick.AddListener(LoadLevel);
        _buttonClose.onClick.AddListener(ClosePanelLevel);
    }

    private void OnDisable()
    {
        foreach (var item in _levels)
            item.Clicked -= SelectLevel;

        _buttonPlay.onClick.RemoveListener(LoadLevel);
        _buttonClose.onClick.RemoveListener(ClosePanelLevel);
    }

    private void InitLevels()
    {
        int countCompletedLevel = 0;

        if (Saves.HasKey(SaveController.Params.Level))
            countCompletedLevel = Saves.GetInt(SaveController.Params.Level);

        print(countCompletedLevel + " Count Completed Levels");
        for (int i = 0; i < countCompletedLevel; i++)
            _levels[i].OpenLevel(3);

        _levels[countCompletedLevel].ShowCurrentLevel();

        for (int i = _levels.Count; i > countCompletedLevel; i++)
            _levels[i].CloseLevel();
    }

    private void SelectLevel(LevelOnMap levelOnMap)
    {
        _currentLevel = levelOnMap;

        StartCoroutine(AnimationSize(Vector3.one));
    }

    private void ClosePanelLevel()
    {
        _currentLevel = null;

        StartCoroutine(AnimationSize(Vector3.zero));
    }

    private void LoadLevel()
    {
        if (_currentLevel == null)
            Debug.LogError("CURRENT LEVEL IS NULL");

        _sceneLoader.LoadScene(_currentLevel.IndexScene);
    }

    private IEnumerator AnimationSize(Vector3 targetScale)
    {
        while(_panelStartLevel.localScale != targetScale)
        {
            _panelStartLevel.localScale = Vector3.MoveTowards(_panelStartLevel.localScale, targetScale, _speedAnimation * Time.deltaTime);
            yield return null;
        }
    }
}
