using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CardsHand : MonoBehaviour, IPhaseHandler
{
    [SerializeField] private Phase[] _phases;
    [SerializeField] private int _capacity;

    private ICardApplicable _currentTarget;
    private List<Card> _cards = new List<Card>();

    public Phase[] Phases => _phases;

    public int Capacity => _capacity;
    public bool CanPlaceCard { get; private set; }
    public bool CanTakeCard => _cards.Count < _capacity;
    public ICardApplicable CurrentTarget => _currentTarget;

    public event Action<UnitFriend> Spawned;
    public event Action<PointerEventData, Card> CardTaken;
    public event Action CardDrop;
    public event Action CancelDrop;
    public event Action CardsEmpty;

    private void Awake()
    {
        _cards = GetComponentsInChildren<Card>().ToList();

        if (Saves.HasKey(SaveController.Params.HandCapacity))
            _capacity = Saves.GetInt(SaveController.Params.HandCapacity);
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

    private void RegisterCard(Card card)
    {
        card.BeginDrag += OnBeginDrag;
        card.Drag += OnDrag;
        card.EndDrag += OnEndDrag;
    }

    private void UnRegister(Card card)
    {
        card.BeginDrag -= OnBeginDrag;
        card.Drag -= OnDrag;
        card.EndDrag -= OnEndDrag;
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        CardTaken?.Invoke(eventData, card);
    }

    private void OnDrag(PointerEventData eventData, Card card)
    {
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
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

    private void OnEndDrag(PointerEventData eventData, Card card)
    {
        bool result = TryApply(card, eventData.position);

        if (result == false)
        {
            card.CancleDrop();
            CancelDrop?.Invoke();
        }

        CardDrop?.Invoke();

        if (_cards.Count <= 0)
            CardsEmpty?.Invoke();
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
                        UnRegister(card);
                        _cards.Remove(card);
                        card.DropOut(mousePosition);
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
