using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Card))]
public class CardHoverView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const float LerpTime = 10f;
    private const float DistanceDelta = 0.1f;

    private Coroutine _coroutine;
    private Card _card;

    public event Action<CardHoverView> Enter;
    public event Action<CardHoverView> Exit;
    public event Action<CardHoverView> BeginDrag;
    public event Action<CardHoverView> CancelDrop;
    public event Action<CardHoverView> Drop;

    public Vector3 StartPosition { get; private set; }
    public int StartIndex { get; private set; }
    public bool CanHover { get; private set; } = true;

    private void Awake()
    {
        _card = GetComponent<Card>();
    }

    private void OnEnable()
    {
        _card.BeginDrag += OnBeginDrag;
        _card.Drop += OnDrop;
        _card.CancelDrop += OnCancelDrag;
    }

    private void OnDisable()
    {
        _card.BeginDrag -= OnBeginDrag;
        _card.Drop -= OnDrop;
        _card.CancelDrop -= OnCancelDrag;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit?.Invoke(this);
    }

    public void SaveStartState(Vector3 position, int index)
    {
        StartPosition = position;
        StartIndex = index;
    }

    public void ResetToStartState()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Lerp(StartPosition));
        transform.SetSiblingIndex(StartIndex);
    }

    public void MoveTo(Vector3 position, Action onEndCallback = null)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Lerp(position, onEndCallback));
    }

    public void BringForward()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        CanHover = false;
        BeginDrag?.Invoke(this);
    }

    private void OnCancelDrag(Card card)
    {
        CanHover = true;
        CancelDrop?.Invoke(this);
    }

    private void OnDrop(Card card)
    {
        CanHover = true;
        Drop?.Invoke(this);
    }

    private IEnumerator Lerp(Vector3 to, Action onEndCallback = null)
    {
        onEndCallback?.Invoke();

        while (Vector3.Distance(transform.position, to) > DistanceDelta)
        {
            transform.position = Vector3.Lerp(transform.position, to, LerpTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        transform.position = to;
        _coroutine = null;
    }
}
