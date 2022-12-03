using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private int _amount;
    [SerializeField] private CardSave _cardSave;
    [SerializeField] private CardName _name;

    public int Amount { get => _amount; protected set => _amount = value; }
    public CardName Name { get => _name; protected set => _name = value; }
    public bool IsActive { get; private set; }
    public CardSave CardSave => _cardSave;
    public virtual Projection ProjectionPrefab { get; }

    public event Action<PointerEventData, Card> Clicked;
    public event Action<PointerEventData, Card> Drag;
    public event Action<PointerEventData, Card> BeginDrag;
    public event Action<PointerEventData, Card> EndDrag;

    public event Action<Card, Vector3> Drop;
    public event Action<Card> CancelDrop;
    public event Action<int> Used;
    public event Action<CardSave> Saved;

    public void Activate(bool isActive)
    {
        IsActive = isActive;
    }

    public void UseOne()
    {
        Amount--;
        Used?.Invoke(Amount);
    }

    public void DropOut(Vector3 mousePosition)
    {
        Amount--;
        Drop?.Invoke(this, mousePosition);
        gameObject.SetActive(false);
    }

    public void CancleDrop()
    {
        CancelDrop?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsActive)
            BeginDrag?.Invoke(eventData, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsActive)
            Drag?.Invoke(eventData, this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsActive)
            EndDrag?.Invoke(eventData, this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Clicked?.Invoke(eventData, this);
    }

    public void Save()
    {
        Saves.SetCard(Name.ToString(), CardSave);
        Saves.Save();

        Saved?.Invoke(_cardSave);
    }

    public void Save(DeckType deck)
    {
        _cardSave.SetDeck(deck);

        Saves.SetCard(Name.ToString(), CardSave);
        Saves.Save();

        Saved?.Invoke(_cardSave);
    }

    public void Load()
    {
        if (Saves.HasKey(Name.ToString()))
            _cardSave = Saves.GetCard(Name.ToString());

        if (CardSave.IsAvailable == false)
            gameObject.SetActive(false);
    }
}
