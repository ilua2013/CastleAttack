using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projector : MonoBehaviour
{
    [SerializeField] private CardsHand _cardsHand;

    private PointerEventData _pointer;
    private Projection _projection;

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
            {
                if (_cardsHand.CanPlaceCard)
                {
                    _projection.gameObject.SetActive(true);
                    _projection.Show(_cardsHand.CurrentTarget.Cell);
                    _projection.transform.position = _cardsHand.CurrentTarget.SpawnPoint;
                }
                else
                {
                    _projection.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnCardTaken(PointerEventData eventData, Card card)
    {
        if (_projection != null)
            Destroy(_projection.gameObject);

        _pointer = eventData;
        _projection = Instantiate(card.ProjectionPrefab);

        _projection.gameObject.SetActive(false);
    }

    private void OnCardDrop()
    {
        _pointer = null;

        if (_projection != null)
            Destroy(_projection.gameObject);

        _projection = null;
    }
}
