using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Projector : MonoBehaviour
{
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private GameObject _cross;

    private PointerEventData _pointer;
    private Projection _projection;
    private GameObject _projectionCross;

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
                    _projectionCross.SetActive(false);
                    _projection.transform.position = _cardsHand.CurrentTarget.SpawnPoint;
                }
                else
                {
                    _projection.gameObject.SetActive(false);
                    _projectionCross.SetActive(true);
                    _projectionCross.transform.position = hit.point;
                }
            }
        }
    }

    private void OnCardTaken(PointerEventData eventData, Card card)
    {
        _pointer = eventData;
        _projection = Instantiate(card.ProjectionPrefab);
        _projectionCross = Instantiate(_cross);

        _projection.gameObject.SetActive(false);
        _projectionCross.SetActive(false);
    }

    private void OnCardDrop()
    {
        Debug.Log("Drop card");
        _pointer = null;

        Destroy(_projection.gameObject);
        Destroy(_projectionCross.gameObject);

        _projection = null;
        _projectionCross = null;
    }
}
