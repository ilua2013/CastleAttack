using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitProjection : MonoBehaviour
{
    [SerializeField] private CardsHand _cardsHand;

    private PointerEventData _pointer;
    private GameObject _projection;

    private void OnEnable()
    {
        _cardsHand.CardTaken += OnCardTaken;
        _cardsHand.CardDrop += OnCardDrop;
        _cardsHand.CancelDrop += OnCardDrop;
    }

    private void OnDisable()
    {
        _cardsHand.CardTaken -= OnCardTaken;
        _cardsHand.CardDrop -= OnCardDrop;
        _cardsHand.CancelDrop -= OnCardDrop;
    }

    private void Update()
    {
        if (_pointer != null && _projection != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(_pointer.position);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
                _projection.transform.position = hit.point;
        }
    }

    private void OnCardTaken(PointerEventData eventData, Card card)
    {
        _pointer = eventData;
        _projection = Instantiate(card.ProjectionPrefab);
    }

    private void OnCardDrop()
    {
        _pointer = null;

        Destroy(_projection);
        _projection = null;
    }
}
