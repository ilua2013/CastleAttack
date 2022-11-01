using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int _amount;

    private Coroutine _coroutine;
    private bool _isDragging;
    private Vector3 _initialPosition;

    public Vector3 InitialPosition => _initialPosition;
    public bool IsDragging => _isDragging;

    public int Amount => _amount;

    public event Action<PointerEventData, Card> Drag;
    public event Action<PointerEventData, Card> BeginDrag;
    public event Action<PointerEventData, Card> EndDrag;
    public event Action<PointerEventData, Card> Enter;
    public event Action<PointerEventData, Card> Exit;
    public event Action<int> Used;
    public event Action<Card> CameBack;

    public void InitPosition(Vector3 pos)
    {
        _initialPosition = pos;
        _isDragging = false;
    }

    public void UseOne()
    {
        _amount--;
        Used?.Invoke(_amount);
    }

    public void DropOut()
    {
        gameObject.SetActive(false);
    }

    public void ComeBack()
    {
        gameObject.SetActive(true);
        CameBack?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        BeginDrag?.Invoke(eventData, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag?.Invoke(eventData, this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _isDragging = false;
        EndDrag?.Invoke(eventData, this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Enter?.Invoke(eventData, this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exit?.Invoke(eventData, this);
    }

    public void LerpPosition(Vector3 end, float time, Action onEndCallback = null)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Lerp(transform, end, time, onEndCallback));
    }

    public void ResetPosition(float time)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Lerp(transform, _initialPosition, time));
    }

    private IEnumerator Lerp(Transform card, Vector3 to, float time, Action onEndCallback = null)
    {
        onEndCallback?.Invoke();

        while (Vector3.Distance(card.position, to) > 0.1f)
        {
            card.position = Vector3.Lerp(card.position, to, time * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        card.position = to;
        _coroutine = null;
    }
}
