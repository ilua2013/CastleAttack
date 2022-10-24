using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsMover : MonoBehaviour
{
    [SerializeField] private List<Card> _cards;
    [SerializeField] private Transform _draggingParent;

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

    private void OnDrag(Vector3 mousePosition, Card card)
    {
        card.transform.position = mousePosition;
    }

    private void OnBeginDrag(Vector3 mousePosition, Card card)
    {
        card.transform.SetParent(_draggingParent);
    }

    private void OnEndDrag(Vector3 mousePosition, Card card)
    {
        if (IsCardApplicable(out ICardApplicable cardApplicable, out Vector3 hitPoint, mousePosition))
        {
            if (cardApplicable.TryApply(card.Description, hitPoint))
            {
                UnRegister(card);
                card.DropOut();

                return;
            }
        }

        card.transform.SetParent(transform);
    }

    private bool IsCardApplicable(out ICardApplicable cardApplicable, out Vector3 hitPoint, Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        if (hits.Length > 0)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent(out ICardApplicable applicable))
                {
                    hitPoint = hit.point;
                    cardApplicable = applicable;
                    return true;
                }
            }
        }

        hitPoint = Vector3.zero;
        cardApplicable = null;
        return false;
    }
}
