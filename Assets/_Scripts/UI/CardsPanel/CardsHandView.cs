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
    private const float OffsetXFactor = 20f;

    private float _offsetX = 250f;

    private List<CardHoverView> _cards;
    private List<CardMovement> _cardMovements = new List<CardMovement>();
    private CardsInHandComparer _comparer = new CardsInHandComparer();

    private void Awake()
    {
        _cards = GetComponentsInChildren<CardHoverView>().ToList();
    }

    private void OnEnable()
    {
        foreach (CardHoverView card in _cards)
        {
            Register(card);
        }
    }

    private void OnDisable()
    {
        foreach (CardHoverView card in _cards)
        {
            UnRegister(card);
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
        float number = -(float)(_cards.Count - 1) / 2;
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

    private Vector3 CalculatePadding(float number, Vector3 position, float offsetX)
    {
        Vector3 padding = position + Vector3.right * offsetX * number;

        if (number > 0)
            padding = padding + Vector3.down * -OffsetY * number;
        else if (number == 0)
            padding = padding + Vector3.down * 0 * number;
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

    public void Register(CardHoverView cardHover)
    {
        cardHover.Enter += OnHover;
        cardHover.Exit += OnRemoveHover;
        cardHover.BeginDrag += OnBeginDrag;
        cardHover.Drop += OnDrop;
        cardHover.CancelDrop += OnCancelDrop;
        cardHover.Used += OnUsed;

        if (cardHover.TryGetComponent(out CardMovement movement))
            _cardMovements.Add(movement);
    }

    public void UnRegister(CardHoverView cardHover)
    {
        cardHover.Enter -= OnHover;
        cardHover.Exit -= OnRemoveHover;
        cardHover.BeginDrag -= OnBeginDrag;
        cardHover.Drop -= OnDrop;
        cardHover.CancelDrop -= OnCancelDrop;
        cardHover.Used -= OnUsed;

        if (cardHover.TryGetComponent(out CardMovement movement))
            _cardMovements.Remove(movement);
    }

    public void CardAdd(Card card)
    {
        CardHoverView cardHover = card.GetComponent<CardHoverView>();
        Register(cardHover);

        _cards.Add(cardHover);
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
