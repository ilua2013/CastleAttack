using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsHand : MonoBehaviour
{
    private List<Card> _cards = new List<Card>();

    public event Action<UnitFriend> Spawned;
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
        card.BeginDrag += OnBeginDrag;
        card.EndDrag += OnEndDrag;
    }

    private void UnRegister(Card card)
    {
        card.BeginDrag -= OnBeginDrag;
        card.EndDrag -= OnEndDrag;
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        CardTaken?.Invoke();
    }

    private void OnEndDrag(PointerEventData eventData, Card card)
    {
        bool result = TryApply(card, eventData.position);

        if (result == false)
        {
            card.CancleDrop();
        }

        CardDrop?.Invoke();
    }

    public void CardAdd(Card card)
    {
        _cards.Add(card);
        RegisterCard(card);
    }

    public void CardComeBack(Card card)
    {
        card.gameObject.SetActive(true);
        RegisterCard(card);
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
                    if (card.Amount <= 1)
                    {
                        UnRegister(card);
                        card.DropOut(mousePosition);
                    }
                    else
                    {
                        card.UseOne();
                    }

                    Spawned?.Invoke(applicable.Spawned);

                    return true;
                }
            }
        }

        return false;
    }
}
