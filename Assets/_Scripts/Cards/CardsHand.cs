using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CardsHand : MonoBehaviour, IPhaseHandler
{
    [SerializeField] private Phase[] _phases;

    private ICardApplicable _currentTarget;
    private Card _selectable;
    private List<Card> _cards = new List<Card>();

    public Phase[] Phases => _phases;

    public bool CanPlaceCard { get; private set; }
    public ICardApplicable CurrentTarget => _currentTarget;

    public event Action<UnitFriend> Spawned;
    public event Action<PointerEventData, Card> CardTaken;
    public event Action CardDrop;
    public event Action CancelDrop;
    public event Action CardsEmpty;

    private void Awake()
    {
        _cards = GetComponentsInChildren<Card>().ToList();
    }

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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _selectable != null)
            TryApplyCard(_selectable, Input.mousePosition);

        if (_selectable != null)
            CheckApplicablePlace(Input.mousePosition, _selectable);
    }

    private void CheckApplicablePlace(Vector3 mousePosition, Card card)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        if (hits.Length == 0)
            return;

        foreach (var hit in hits)
        {
            ICardApplicable[] applicables = hit.collider.GetComponents<ICardApplicable>();

            foreach (var applicable in applicables)
            {
                if (applicable.CanApply(card))
                {
                    _currentTarget = applicable;
                    CanPlaceCard = true;
                    return;
                }
            }
        }

        CanPlaceCard = false;
    }

    private void TryApplyCard(Card card, Vector3 mousePosition)
    {
        bool result = TryApply(card, mousePosition);

        if (result == false)
        {
            _selectable = null;
            card.CancleDrop();
            CancelDrop?.Invoke();
        }

        if (_cards.Count <= 0)
            CardsEmpty?.Invoke();
    }

    private void RegisterCard(Card card)
    {
        card.Clicked += OnClicked;
        card.BeginDrag += OnBeginDrag;
        card.Drag += OnDrag;
        card.EndDrag += OnEndDrag;
    }

    private void UnRegister(Card card)
    {
        card.Clicked -= OnClicked;
        card.BeginDrag -= OnBeginDrag;
        card.Drag -= OnDrag;
        card.EndDrag -= OnEndDrag;
    }

    private void OnClicked(PointerEventData eventData, Card card)
    {
        CardTaken?.Invoke(eventData, card);
        _selectable = card;
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        if (_selectable == null)
            CardTaken?.Invoke(eventData, card);
    }

    private void OnDrag(PointerEventData eventData, Card card)
    {
        CheckApplicablePlace(eventData.position, card);
    }

    private void OnEndDrag(PointerEventData eventData, Card card)
    {
        if (_selectable == null)
            TryApplyCard(card, eventData.position);
    }

    public IEnumerator SwitchPhase(PhaseType phaseType)
    {
        Phase phase = _phases.FirstOrDefault((phase) => phase.PhaseType == phaseType);

        yield return new WaitForSeconds(phase.Delay);

        foreach (Card card in _cards)
            card.Activate(phase.IsActive);
    }

    public void CardAdd(Card card)
    {
        _cards.Add(card);
        RegisterCard(card);
    }

    public void CardComeBack(Card card)
    {
        _cards.Add(card);
        card.gameObject.SetActive(true);
        RegisterCard(card);
    }

    private bool TryApply(Card card, Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        if (hits.Length == 0)
            return false;

        foreach (var hit in hits)
        {
            ICardApplicable[] applicables = hit.collider.GetComponents<ICardApplicable>();

            foreach (var applicable in applicables)
            {
                if (applicable.TryApplyFriend(card, hit.point))
                {
                    if (card.Amount <= 1)
                    {
                        _cards.Remove(card);
                        UnRegister(card);
                        card.DropOut(mousePosition);
                        CardDrop?.Invoke();

                        _selectable = null;
                    }
                    else
                    {
                        card.UseOne();
                    }

                    Spawned?.Invoke(applicable.Spawned);

                    return true;
                }
            }
        }

        return false;
    }
}
