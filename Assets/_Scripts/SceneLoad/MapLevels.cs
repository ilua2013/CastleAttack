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
    [SerializeField] private Button _buttonClose;
    [SerializeField] private RectTransform _content;
    [Header("Animations Current Level")]
    [SerializeField] private Animator _animatorCurrentLevel;
    [Header("Panel Start Level")]
    [SerializeField] private PanelStartLevelView _panelStartLevel;
    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonClosePanel;
    [SerializeField] private float _speedAnimation;
    [Header("OnValidate")]
    [SerializeField] private bool _setTransformToZero = true;

    private LevelOnMap _currentLevel;

    private void OnValidate()
    {
        _levels = GetComponentsInChildren<LevelOnMap>().ToList();
        _sceneLoader = FindObjectOfType<SceneLoader>();

        if (_panelStartLevel == null)
            _panelStartLevel = FindObjectOfType<PanelStartLevelView>(true);

        if (_setTransformToZero)
            transform.localScale = Vector3.zero;

        int indexNumber = _firstIndexLevel;

        foreach (var item in _levels)
        {
            item.LevelNumber = indexNumber;
            indexNumber++;
        }
    }

    private void Awake()
    {
        _panelStartLevel.transform.localScale = Vector3.zero;
        transform.localScale = Vector3.zero;
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
        _buttonClosePanel.onClick.AddListener(ClosePanelLevel);
        _buttonClose.onClick.AddListener(CloseMap);
    }

    private void OnDisable()
    {
        foreach (var item in _levels)
            item.Clicked -= SelectLevel;

        _buttonPlay.onClick.RemoveListener(LoadLevel);
        _buttonClosePanel.onClick.RemoveListener(ClosePanelLevel);
        _buttonClose.onClick.RemoveListener(CloseMap);
    }

    public void OpenMap()
    {
        StartCoroutine(AnimationSize(transform, Vector3.one));
    }

    public void CloseMap()
    {
        StartCoroutine(AnimationSize(transform, Vector3.zero));
    }

    private void InitLevels()
    {
        int countCompletedLevel = 0;

        if (Saves.HasKey(SaveController.Params.CompletedLevel))
            countCompletedLevel = Saves.GetInt(SaveController.Params.CompletedLevel);

        for (int i = 0; i < countCompletedLevel; i++)
            _levels[i].OpenLevel(3);

        _levels[countCompletedLevel].ShowCurrentLevel();

        _animatorCurrentLevel.transform.parent = _levels[countCompletedLevel].transform;
        _animatorCurrentLevel.transform.localPosition = new Vector3(0, 21,0);

        for (int i = _levels.Count - 1; i > countCompletedLevel; i--)
            _levels[i].CloseLevel();
    }

    private void SelectLevel(LevelOnMap levelOnMap)
    {
        _currentLevel = levelOnMap;
        Saves.SelectedLevel = levelOnMap.Level;

        _panelStartLevel.Init(levelOnMap.Level);
        StartCoroutine(AnimationSize(_panelStartLevel.transform, Vector3.one));
    }

    private void ClosePanelLevel()
    {
        _currentLevel = null;
        Saves.SelectedLevel = 0;

        StartCoroutine(AnimationSize(_panelStartLevel.transform, Vector3.zero));
    }

    private void LoadLevel()
    {
        if (_currentLevel == null)
            Debug.LogError("CURRENT LEVEL IS NULL");

        _panelStartLevel.gameObject.SetActive(false);
        _sceneLoader.LoadLevel(_currentLevel.Level);
    }

    private IEnumerator AnimationSize(Transform rectTransform, Vector3 targetScale)
    {
        while (rectTransform.localScale != targetScale)
        {
            rectTransform.localScale = Vector3.MoveTowards(rectTransform.localScale, targetScale, _speedAnimation * Time.deltaTime);
            _content.localPosition = Vector3.zero;
            yield return null;
        }
    }
}
