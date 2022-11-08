using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private CardDescription _description;

    public CardDescription Description => _description;

    public event Action<PointerEventData, Card> Drag;
    public event Action<PointerEventData, Card> BeginDrag;
    public event Action<PointerEventData, Card> EndDrag;
    public event Action<Card> CameBack;
<<<<<<< Updated upstream:Assets/_Scripts/UI/Card.cs
=======
    public event Action<int> Used;

    public int Amount => _amount;

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
>>>>>>> Stashed changes:Assets/_Scripts/UI/Card/Card.cs

    public void DropOut()
    {
        gameObject.SetActive(false);
    }

    public void ComeBack()
    {
        _amount++;
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
