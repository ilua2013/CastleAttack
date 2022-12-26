using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionListView : MonoBehaviour
{
    enum Direction
    {
        Right = 1,
        Left = 0,
    }

    private const float LerpTime = 10f;
    private const float DistanceDelta = 0.01f;

    [SerializeField] private ScrollRect _scrollRect1;
    [SerializeField] private ScrollRect _scrollRect2;
    [SerializeField] private Button _left;
    [SerializeField] private Button _right;
    [SerializeField] private TMP_Text _pages;

    private string _initialText;
    private Coroutine _coroutine;

    private void Start()
    {
        _initialText = _pages.text;
        _pages.text = _initialText + " " + 1;
    }

    private void OnEnable()
    {
        _left.onClick.AddListener(OnLeftClick);
        _right.onClick.AddListener(OnRightClick);
    }

    private void OnDisable()
    {
        _left.onClick.RemoveListener(OnLeftClick);
        _right.onClick.RemoveListener(OnRightClick);
    }

    private void OnRightClick()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _pages.text = _initialText + " " + 2;
        _coroutine = StartCoroutine(Slide(new Vector2(((float)Direction.Right), _scrollRect1.normalizedPosition.y)));
    }

    private void OnLeftClick()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _pages.text = _initialText + " " + 1;
        _coroutine = StartCoroutine(Slide(new Vector2(((float)Direction.Left), _scrollRect1.normalizedPosition.y)));
    }

    private IEnumerator Slide(Vector2 to)
    {
        while (Vector3.Distance(_scrollRect1.normalizedPosition, to) > DistanceDelta)
        {
            _scrollRect1.normalizedPosition = Vector3.Lerp(_scrollRect1.normalizedPosition, to, LerpTime * Time.deltaTime);
            _scrollRect2.normalizedPosition = Vector3.Lerp(_scrollRect2.normalizedPosition, to, LerpTime * Time.deltaTime);

            yield return null;
        }

        _scrollRect1.normalizedPosition = to;
        _scrollRect2.normalizedPosition = to;
    }
}
