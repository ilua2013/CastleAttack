using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private int _amount;

    public event Action<PointerEventData, Card> Drag;
    public event Action<PointerEventData, Card> BeginDrag;
    public event Action<PointerEventData, Card> EndDrag;

    public event Action<Card> Drop;
    public event Action<Card> CancelDrop;
    public event Action<Card> CameBack;
    public event Action<int> Used;

    public int Amount => _amount;

    public void UseOne()
    {
        _amount--;
        Used?.Invoke(_amount);
    }

    public void DropOut()
    {
        Drop?.Invoke(this);
        gameObject.SetActive(false);
    }

    public void CancleDrop()
    {
        CancelDrop?.Invoke(this);
    }

    public void ComeBack()
    {
        gameObject.SetActive(true);
        CameBack?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDrag?.Invoke(eventData, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag?.Invoke(eventData, this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag?.Invoke(eventData, this);
    }
}
