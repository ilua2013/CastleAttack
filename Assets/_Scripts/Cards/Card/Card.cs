using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private int _amount;

    public int Amount { get => _amount; protected set => _amount = value; }

    public event Action<PointerEventData, Card> Clicked;
    public event Action<PointerEventData, Card> Drag;
    public event Action<PointerEventData, Card> BeginDrag;
    public event Action<PointerEventData, Card> EndDrag;

    public event Action<Card, Vector3> Drop;
    public event Action<Card> CancelDrop;
    public event Action<int> Used;

    public void UseOne()
    {
        Amount--;
        Used?.Invoke(Amount);
    }

    public void DropOut(Vector3 mousePosition)
    {
        Amount--;
        gameObject.SetActive(false);
        Drop?.Invoke(this, mousePosition);
    }

    public void CancleDrop()
    {
        CancelDrop?.Invoke(this);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(eventData, this);
    }
}
