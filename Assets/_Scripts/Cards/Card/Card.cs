using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private int _amount;
    [SerializeField] private CardSave _cardSave;
    [SerializeField] private CardName _name;

    public CardSave CardSave => _cardSave;
    public int Amount { get => _amount; protected set => _amount = value; }
    public CardName Name { get => _name; protected set => _name = value; }
    public bool IsActive { get; private set; }
    public bool IsAvailable { get; private set; }
    public Deck Deck { get; private set; }


    public event Action<PointerEventData, Card> Clicked;
    public event Action<PointerEventData, Card> Drag;
    public event Action<PointerEventData, Card> BeginDrag;
    public event Action<PointerEventData, Card> EndDrag;

    public event Action<Card, Vector3> Drop;
    public event Action<Card> CancelDrop;
    public event Action<int> Used;

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
        gameObject.SetActive(false);
        Drop?.Invoke(this, mousePosition);
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


    public void Save(Deck deck)
    {
        _cardSave = new CardSave(IsAvailable, deck);

        Saves.SetCard(Name.ToString(), CardSave);
        Saves.Save();
    }

    public void Load()
    {
        if (Saves.HasKey(Name.ToString()))
            _cardSave = Saves.GetCard(Name.ToString());

        IsAvailable = CardSave.IsAvailable;
        Deck = CardSave.Deck;

        if (CardSave.IsAvailable == false)
            gameObject.SetActive(false);
    }
}
