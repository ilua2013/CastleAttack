using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardsHandView : MonoBehaviour
{
    [SerializeField] private CardsMover _cardsMover;

    private const float _offsetY = -4f;
    private const float _offsetX = 100f;
    private const float _radius = 3.89f;
    private const float _angle = 1.33f;

    private float _offsetYLeft;

    private void Awake()
    {
        _offsetYLeft = -_offsetY;
    }

    private void Start()
    {
        _cardsMover.CardTaken += Shuffling;
        _cardsMover.CardDrop += Shuffling;

        foreach (var card in _cardsMover.CardsInHand)
        {
            card.Enter += OnCardSelect;
            card.Exit += OnCardDeselect;
        }

        Shuffling();
    }

    private void OnDisable()
    {
        _cardsMover.CardTaken -= Shuffling;
        _cardsMover.CardDrop -= Shuffling;

        foreach (var card in _cardsMover.CardsInHand)
        {
            card.Enter -= OnCardSelect;
            card.Exit -= OnCardDeselect;
        }
    }

    private void Shuffling()
    {
        Vector3 center = transform.position;
        int number = (-_cardsMover.CardsCount + 1) / 2;

        foreach (Card card in _cardsMover.CardsInHand)
        {
            Vector3 position = RandomCircle(transform.position, _radius, number);
            Quaternion rotation = Quaternion.FromToRotation(Vector3.down, transform.position - position);

            Vector3 lerpPosition = position + Vector3.right * _offsetX * number;

            if (number > 0)
                lerpPosition = lerpPosition  + Vector3.down * _offsetYLeft * number;
            else
                lerpPosition = lerpPosition  + Vector3.down * _offsetY * number;

            card.transform.rotation = rotation;
            card.LerpPosition(lerpPosition, 15f, () => card.InitPosition(lerpPosition));

            number++;
        }
    }

    private Vector3 RandomCircle(Vector3 center, float radius, float ang)
    {
        Vector3 position = new();

        position.x = center.x + radius * _radius * Mathf.Sin(ang * _angle * Mathf.Deg2Rad);
        position.y = center.y + radius * _radius * Mathf.Cos(ang * _angle * Mathf.Deg2Rad);
        position.z = center.z;

        return position;
    }

    private void OnCardSelect(PointerEventData eventData, Card card)
    {
        if (card.IsDragging)
            return;

        card.LerpPosition(card.InitialPosition + Vector3.up * 20f, 500f);
    }

    private void OnCardDeselect(PointerEventData eventData, Card card)
    {
        if (card.IsDragging)
            return;

        card.ResetPosition(500f);
    }
}
