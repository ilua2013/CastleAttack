using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsMover : MonoBehaviour
{
    [SerializeField] private Transform _draggingParent;

    private List<Card> _cards = new List<Card>();

    public event Action CardTaken;
    public event Action CardDrop;

    private void Awake()
    {
        _cards = GetComponentsInChildren<Card>().ToList();
    }

    private void OnEnable()
    {
        foreach (var card in _cards)
            RegisterCard(card);
    }

    private void OnDisable()
    {
        foreach (var card in _cards)
            UnRegister(card);
    }

    private void RegisterCard(Card card)
    {
        card.Drag += OnDrag;
        card.BeginDrag += OnBeginDrag;
        card.EndDrag += OnEndDrag;
    }

    private void UnRegister(Card card)
    {
        card.Drag -= OnDrag;
        card.BeginDrag -= OnBeginDrag;
        card.EndDrag -= OnEndDrag;
    }

    private void OnDrag(PointerEventData eventData, Card card)
    {
        if (IsOverDraggingPanel(eventData))
            card.transform.position = eventData.position;
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        card.transform.SetParent(_draggingParent);

        CardTaken?.Invoke();
    }

    private void OnEndDrag(PointerEventData eventData, Card card)
    {
        bool result = TryApply(card, eventData.position);

        if (result == false)
        {
            card.transform.SetParent(transform);
            card.CancleDrop();
        }

        CardDrop?.Invoke();
    }

    private void OnCardComeBack(Card card)
    {
        RegisterCard(card);
        card.CameBack -= OnCardComeBack;
        card.transform.SetParent(transform);
    }

    private bool IsOverDraggingPanel(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var item in results)
        {
            if (!item.isValid)
                continue;

            if (item.gameObject.transform == _draggingParent)
                return true;
        }

        return false;
    }

    private bool TryApply(Card card, Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        if (hits.Length == 0)
            return false;

        foreach (var hit in hits)
        {
            ICardApplicable[] applicables = hit.collider.GetComponents<ICardApplicable>();

            foreach (var applicable in applicables)
            {
                if (applicable.TryApplyFriend(card, hit.point))
                {
                    card.UseOne();

                    if (card.Amount <= 0)
                    {
                        UnRegister(card);
                        card.DropOut();
                        card.CameBack += OnCardComeBack;
                    }
                    else
                    {
                        card.CancleDrop();
                        card.transform.SetParent(transform);
                    }

                    return true;
                }
            }
        }

        return false;
    }
}
