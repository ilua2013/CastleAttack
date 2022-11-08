using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CardsHandView : MonoBehaviour
{
    private const float OffsetY = -1f;
    private const float OffsetX = 150f;
    private const float Radius = 3.89f;
    private const float Angle = 1.33f;
    private const float ScaleFactor = 1.2f;

    private List<CardHoverView> _cards;
    private CardsInHandComparer _comparer = new CardsInHandComparer();

    private void Awake()
    {
        _cards = GetComponentsInChildren<CardHoverView>().ToList();
    }

    private void OnEnable()
    {
        foreach (CardHoverView card in _cards)
        {
            card.Enter += OnHover;
            card.Exit += OnRemoveHover;
            card.BeginDrag += OnBeginDrag;
            card.Drop += OnDrop;
            card.CancelDrop += OnCancelDrop;
            card.CameBack += OnCameBack;
            card.Used += OnUsed;
        }
    }

    private void OnDisable()
    {
        foreach (CardHoverView card in _cards)
        {
            card.Enter -= OnHover;
            card.Exit -= OnRemoveHover;
            card.BeginDrag -= OnBeginDrag;
            card.Drop -= OnDrop;
            card.CancelDrop -= OnCancelDrop;
            card.CameBack -= OnCameBack;
            card.Used -= OnUsed;
        }
    }

    private void Start()
    {
        Shuffling();
    }

    private void Shuffling()
    {
        Vector3 center = transform.position;
        int number = -_cards.Count / 2;

        _cards.Sort(_comparer);

        foreach (CardHoverView card in _cards)
        {
            card.ResetToStartState();

            Vector3 position = CalculatePosition(transform.position, Radius, number);
            Vector3 lerpPosition = CalculatePadding(number, position);

            Quaternion rotation = Quaternion.FromToRotation(Vector3.down, transform.position - position);

            card.transform.rotation = rotation;
            card.SaveStartState(lerpPosition, card.transform.GetSiblingIndex());
            card.MoveTo(lerpPosition);

            number++;
        }
    }

    private Vector3 CalculatePadding(int number, Vector3 position)
    {
        Vector3 padding = position + Vector3.right * OffsetX * number;

        if (number > 0)
            padding = padding + Vector3.down * -OffsetY * number;
        else
            padding = padding + Vector3.down * OffsetY * number;

        return padding;
    }

    private Vector3 CalculatePosition(Vector3 center, float radius, float ang)
    {
        Vector3 position = new();

        position.x = center.x + radius * Radius * Mathf.Sin(ang * Angle * Mathf.Deg2Rad);
        position.y = center.y + radius * Radius * Mathf.Cos(ang * Angle * Mathf.Deg2Rad);
        position.z = center.z;

        return position;
    }

    private void OnHover(CardHoverView card)
    {
        if (card.CanHover)
        {
            Vector3 hoverPosition = card.StartPosition + Vector3.up * 10f;

            card.MoveTo(hoverPosition);
            card.ScaleTo(card.StartScaling * ScaleFactor);
            card.BringForward();
        }
    }

    private void OnRemoveHover(CardHoverView card)
    {
        if (card.CanHover)
        {
            card.ResetToStartState();
        }
    }

    private void OnBeginDrag(CardHoverView card)
    {
        _cards.Remove(card);
        Shuffling();
    }

    private void OnDrop(CardHoverView card)
    {
        _cards.Remove(card);
        Shuffling();
    }

    private void OnCancelDrop(CardHoverView card)
    {
        _cards.Add(card);
        Shuffling();
    }

    private void OnCameBack(CardHoverView card)
    {
        if (card.Card.Amount <= 1)
        {
            _cards.Add(card);
            Shuffling();
        }
    }

    private void OnUsed(CardHoverView card, int count)
    {
        _cards.Add(card);
        Shuffling();
    }
}
