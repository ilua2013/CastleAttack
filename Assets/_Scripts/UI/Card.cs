using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private CardDescription _description;

    public CardDescription Description => _description;

    public event Action<Vector3, Card> Drag;
    public event Action<Vector3, Card> BeginDrag;
    public event Action<Vector3, Card> EndDrag;

    public void DropOut()
    {
        Destroy(gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BeginDrag?.Invoke(eventData.position, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Drag?.Invoke(eventData.position, this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EndDrag?.Invoke(eventData.position, this);
    }
}
