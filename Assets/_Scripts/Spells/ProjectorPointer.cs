using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorPointer : MonoBehaviour
{
    [SerializeField] private GameObject _pointerPrefab;

    private Transform _pointer;
    private CardsMover _cardsMover;

    private bool _isCardInHand;

    private void Awake()
    {
        _pointer = Instantiate(_pointerPrefab).transform;
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
        {
            _pointer.gameObject.SetActive(false);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        if (hits.Length == 0)
            _pointer.gameObject.SetActive(false);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out ICardApplicable applicable))
            {
                _pointer.gameObject.SetActive(true);
                _pointer.position = hit.point + Vector3.up * 0.1f;
            }
        }
    }

    private void OnCardTaken()
    {
        _isCardInHand = true;
    }

    private void OnCardDrop()
    {
        _isCardInHand = false;
    }
}
