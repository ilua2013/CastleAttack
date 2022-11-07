using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorPointer : MonoBehaviour
{
    private HighlightingCell _previous;
    private CardsMover _cardsMover;

    private bool _isCardInHand;

    private void Awake()
    {
        _cardsMover = FindObjectOfType<CardsMover>();
    }

    private void OnEnable()
    {
        _cardsMover.CardTaken += OnCardTaken;
        _cardsMover.CardDrop += OnCardDrop;
    }

    private void OnDisable()
    {
        _cardsMover.CardTaken -= OnCardTaken;
        _cardsMover.CardDrop -= OnCardDrop;
    }

    private void Update()
    {
        if (_isCardInHand == false)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        if (hits.Length == 0)
            return;

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out HighlightingCell cell))
            {
                if (_previous != null)
                    _previous.UnSelect();

                cell.Select();
                _previous = cell;
            }
        }
    }

    private void OnCardTaken()
    {
        _isCardInHand = true;
    }

    private void OnCardDrop()
    {
        if (_previous != null)
            _previous.UnSelect();

        _isCardInHand = false;
    }
}
