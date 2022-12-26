using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Card))]
public class CardHoverView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float LerpTime = 10f;
    private const float DistanceDelta = 0.05f;

    private Coroutine _coroutinePosition;
    private Coroutine _coroutineScaling;
    private Card _card;

    public event Action<CardHoverView> Enter;
    public event Action<CardHoverView> Exit;
    public event Action<CardHoverView> BeginDrag;
    public event Action<CardHoverView> CancelDrop;
    public event Action<CardHoverView> Drop;
    public event Action<CardHoverView> CameBack;
    public event Action<CardHoverView, int> Used;

    public Vector3 StartPosition { get; private set; }
    public Vector3 StartScaling { get; private set; } = new Vector3(2f, 2f, 2f);
    public int StartIndex { get; private set; }
    public bool CanHover { get; private set; } = true;
    public Card Card => _card;

    private void Awake()
    {
        _card = GetComponent<Card>();
        StartIndex = transform.GetSiblingIndex();
    }

    private void OnEnable()
    {
        _card.BeginDrag += OnBeginDrag;
        _card.Drop += OnDrop;
        _card.CancelDrop += OnCancelDrag;
        _card.Used += OnUse;
    }

    private void OnDisable()
    {
        _card.BeginDrag -= OnBeginDrag;
        _card.Drop -= OnDrop;
        _card.CancelDrop -= OnCancelDrag;
        _card.Used -= OnUse;

        transform.localScale = StartScaling;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit?.Invoke(this);
    }

    public void SaveStartIndex(int index)
    {
        StartIndex = index;
    }

    public void SaveStartState(Vector3 position, int index)
    {
        StartPosition = position;
        StartIndex = index;
    }

    public void ResetToStartState()
    {
        CanHover = true;

        if (enabled == false || gameObject.activeInHierarchy == false)
            return;

        if (_coroutinePosition != null)
            StopCoroutine(_coroutinePosition);

        if (_coroutineScaling != null)
            StopCoroutine(_coroutineScaling);

        _coroutinePosition = StartCoroutine(LerpPosition(StartPosition, LerpTime));
        _coroutineScaling = StartCoroutine(LerpScale(StartScaling));

        transform.SetSiblingIndex(StartIndex);
    }

    public void MoveTo(Vector3 position, Action onEndCallback = null)
    {
        if (enabled == false || gameObject.activeInHierarchy == false)
            return;

        if (_coroutinePosition != null)
            StopCoroutine(_coroutinePosition);

        _coroutinePosition = StartCoroutine(LerpPosition(position, LerpTime, onEndCallback));
    }

    public void MoveTo(Vector3 position, float lerpTime, Action onEndCallback = null)
    {
        if (enabled == false || gameObject.activeInHierarchy == false)
            return;

        if (_coroutinePosition != null)
            StopCoroutine(_coroutinePosition);

        _coroutinePosition = StartCoroutine(LerpPosition(position, lerpTime, onEndCallback));
    }

    public void ScaleTo(Vector3 scale)
    {
        if (enabled == false || gameObject.activeInHierarchy == false)
            return;

        if (_coroutineScaling != null)
            StopCoroutine(_coroutineScaling);

        _coroutineScaling = StartCoroutine(LerpScale(scale));
    }

    public void BringForward()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        if (_coroutinePosition != null)
            StopCoroutine(_coroutinePosition);

        if (_coroutineScaling != null)
            StopCoroutine(_coroutineScaling);

        CanHover = false;
        BeginDrag?.Invoke(this);
    }

    private void OnCancelDrag(Card card)
    {
        CanHover = true;
        CancelDrop?.Invoke(this);
    }

    private void OnDrop(Card card, Vector3 mousePosition)
    {
        CanHover = false;
        Drop?.Invoke(this);
    }

    private void OnUse(int count)
    {
        CanHover = true;
        Used?.Invoke(this, count);
    }

    private IEnumerator LerpPosition(Vector3 to, float time, Action onEndCallback = null)
    {
        while (Vector3.Distance(transform.position, to) > DistanceDelta)
        {
            transform.position = Vector3.MoveTowards(transform.position, to, time * 200 * Time.deltaTime);
            yield return null;
        }

        transform.position = to;
        onEndCallback?.Invoke();
        _coroutinePosition = null;
    }

    private IEnumerator LerpScale(Vector3 to)
    {
        while (Vector3.Distance(transform.localScale, to) > DistanceDelta)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, to, LerpTime * Time.deltaTime);
            yield return null;
        }

        transform.localScale = to;
    }
}
