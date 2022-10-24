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
        bool result = TryApply(card, mousePosition);

        if (result == false)
            card.transform.SetParent(transform);
    }

    private bool TryApply(Card card, Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        if (hits.Length == 0)
            return false;

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out ICardApplicable applicable))
            {
                if (applicable.TryApply(card.Description, hit.point))
                {
                    UnRegister(card);
                    card.DropOut();

                    return true;
                }
            }
        }

        return false;
    }
}
