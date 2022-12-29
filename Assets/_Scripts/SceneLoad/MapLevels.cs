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
    [SerializeField] private RectTransform _panelStartLevel;
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
        _panelStartLevel.localScale = Vector3.zero;

        if (_setTransformToZero)
            transform.localScale = Vector3.zero;

        int indexNumber = _firstIndexLevel;

        foreach (var item in _levels)
        {
            item.IndexScene = indexNumber;
            indexNumber++;
        }
    }

    private void Awake()
    {
        //_panelStartLevel.parent = GetComponentInParent<Canvas>().transform;
        _panelStartLevel.localScale = Vector3.zero;

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

        if (Saves.HasKey(SaveController.Params.Level))
            countCompletedLevel = Saves.GetInt(SaveController.Params.Level) - _firstIndexLevel + 1;

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

        StartCoroutine(AnimationSize(_panelStartLevel,Vector3.one));
    }

    private void ClosePanelLevel()
    {
        _currentLevel = null;

        StartCoroutine(AnimationSize(_panelStartLevel,Vector3.zero));
    }

    private void LoadLevel()
    {
        if (_currentLevel == null)
            Debug.LogError("CURRENT LEVEL IS NULL");

        _panelStartLevel.gameObject.SetActive(false);
        _sceneLoader.LoadScene(_currentLevel.IndexScene);
    }

    private IEnumerator AnimationSize(Transform transform,Vector3 targetScale)
    {
        while(transform.localScale != targetScale)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, _speedAnimation * Time.deltaTime);
            _content.localPosition = Vector3.zero;
            yield return null;
        }
    }
}
