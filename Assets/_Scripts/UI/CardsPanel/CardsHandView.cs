using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CardsHandView : MonoBehaviour
{
    private const float HoverOffsetY = 50f;
    private const float OffsetY = -1f;
    private const float Radius = 3.89f;
    private const float Angle = 1.33f;
    private const float ScaleFactor = 1.2f;
    private const float OffsetXFactor = 2.5f;

    private float _offsetX = 150f;

    private List<CardHoverView> _cards;
    private List<CardMovement> _cardMovements = new List<CardMovement>();
    private CardsInHandComparer _comparer = new CardsInHandComparer();

    private void Awake()
    {
        _cards = GetComponentsInChildren<CardHoverView>().ToList();

        foreach (CardHoverView card in _cards)
        {
            if (card.TryGetComponent(out CardMovement movement))
                _cardMovements.Add(movement);
        }
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
            card.Used -= OnUsed;
        }
    }

    private void Start()
    {
        Shuffling();
    }

    private void Update()
    {
        foreach (CardMovement cardMovement in _cardMovements)
            cardMovement.Move();
    }

    private void Shuffling()
    {
        Vector3 center = transform.position;
        int number = -_cards.Count / 2;
        float offsetX = _offsetX - _cards.Count * OffsetXFactor;

        _cards.Sort(_comparer);

        foreach (CardHoverView card in _cards)
        {
            card.ResetToStartState();

            Vector3 position = CalculatePosition(transform.position, Radius, number);
            Vector3 lerpPosition = CalculatePadding(number, position, offsetX);

            Quaternion rotation = Quaternion.FromToRotation(Vector3.down, transform.position - position);

            card.transform.rotation = rotation;
            card.SaveStartState(lerpPosition, card.transform.GetSiblingIndex());
            card.MoveTo(lerpPosition);

            number++;
        }
    }

    private Vector3 CalculatePadding(int number, Vector3 position, float offsetX)
    {
        Vector3 padding = position + Vector3.right * offsetX * number;

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
            Vector3 hoverPosition = card.StartPosition + Vector3.up * HoverOffsetY;

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

    public void CardAdd(Card card)
    {
        CardHoverView cardHover = card.GetComponent<CardHoverView>();

        cardHover.Enter += OnHover;
        cardHover.Exit += OnRemoveHover;
        cardHover.BeginDrag += OnBeginDrag;
        cardHover.Drop += OnDrop;
        cardHover.CancelDrop += OnCancelDrop;
        cardHover.Used += OnUsed;

        _cards.Add(cardHover);

        if (cardHover.TryGetComponent(out CardMovement movement))
            _cardMovements.Add(movement);

        Shuffling();
    }

    public void CardComeBack(Card card)
    {
        CardHoverView cardHover = card.GetComponent<CardHoverView>();

        _cards.Add(cardHover);
        Shuffling();
    }

    private void OnUsed(CardHoverView card, int count)
    {
        _cards.Add(card);
        Shuffling();
    }
}
