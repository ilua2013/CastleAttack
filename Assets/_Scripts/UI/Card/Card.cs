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

    public event Action<Card, Vector3> Drop;
    public event Action<Card> CancelDrop;
    public event Action<Card> CameBack;
    public event Action<int> Used;

    public int Amount => _amount;
    public CardStage Stage { get; private set; }

    public void UseOne()
    {
        _amount--;
        Used?.Invoke(_amount);
    }

    public void DropOut(Vector3 mousePosition)
    {
        _amount--;
        Drop?.Invoke(this, mousePosition);
    }

    public void CancleDrop()
    {
        CancelDrop?.Invoke(this);
    }

    public void ComeBack()
    {
        _amount++;

        if (Stage != CardStage.Three)
            Stage++;

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
