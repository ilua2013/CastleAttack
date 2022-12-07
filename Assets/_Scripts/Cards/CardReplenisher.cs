using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardReplenisher : MonoBehaviour
{
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private CardsHandView _cardsHandView;

    private List<UnitCard> _unitCards = new List<UnitCard>();
    private List<SpellCard> _spellCards = new List<SpellCard>();

    public event Action<UnitCard, UnitCard> CardUp;

    private void Awake()
    {
        _unitCards = GetComponentsInChildren<UnitCard>().ToList();
    }

    private void OnEnable()
    {
        foreach (UnitCard card in _unitCards)
        {
            card.StageUp += OnCardStageUp;
            card.CameBack += OnCameBack;
        }
    }

    private void OnDisable()
    {
        foreach (UnitCard card in _unitCards)
        {
            card.StageUp -= OnCardStageUp;
            card.CameBack -= OnCameBack;
        }
    }

    public void Create(Card card, Vector2 position)
    {
        if (card is UnitCard unitCard)
            CreateUnit(unitCard, position);
        else if (card is SpellCard spellCard)
            CreateSpell(spellCard, position);
    }

    private void OnCardStageUp(UnitCard card, UnitFriend unit)
    {
        card.StageUp -= OnCardStageUp;
        card.CameBack -= OnCameBack;

        UnitCard newCard = Instantiate(card.NextStage, transform.position, Quaternion.identity, _cardsHand.transform);
        newCard.Init(0);
        newCard.gameObject.SetActive(false);

        UnitFriend newUnit = Instantiate(unit.Card.NextStage.UnitPrefab, unit.transform.position, Quaternion.identity);
        newUnit.Init(newCard, unit.Mover.CurrentCell, unit.CurrentStep);

        unit.LevelUp(newUnit);

        newCard.StageUp += OnCardStageUp;
        newCard.CameBack += OnCameBack;

        _cardsHandView.Register(newCard.GetComponent<CardHoverView>());
        _unitCards.Add(newCard);

        CardUp?.Invoke(card, card.NextStage);
    }

    private bool TryMergeCard(UnitCard card)
    {
        foreach (UnitCard unitCard in _unitCards)
        {
            if (unitCard.UnitPrefab == card.UnitPrefab)
            {
                if (unitCard.gameObject.activeInHierarchy)
                {
                    unitCard.Merge();
                    return true;
                }
            }
        }

        return false;
    }

    private void OnCameBack(UnitCard card)
    {
        if (TryMergeCard(card))
            return;

        _cardsHand.CardComeBack(card);
        _cardsHandView.CardComeBack(card);
    }

    private void CreateUnit(UnitCard card, Vector2 position)
    {
        if (TryMergeCard(card))
            return;

        UnitCard newCard = Instantiate(card, position, Quaternion.identity, _cardsHand.transform);
        newCard.gameObject.SetActive(true);

        newCard.StageUp += OnCardStageUp;
        newCard.CameBack += OnCameBack;

        _unitCards.Add(newCard);
        _cardsHand.CardAdd(newCard, true);
        _cardsHandView.CardAdd(newCard);
    }

    private void CreateSpell(SpellCard card, Vector2 position)
    {
        foreach (SpellCard spellCard in _spellCards)
        {
            if (spellCard.SpellPrefab == card.SpellPrefab)
            {
                if (spellCard.gameObject.activeInHierarchy)
                {
                    spellCard.Merge();
                    return;
                }
            }
        }

        SpellCard newCard = Instantiate(card, position, Quaternion.identity, _cardsHand.transform);
        newCard.gameObject.SetActive(true);

        _spellCards.Add(newCard);
        _cardsHand.CardAdd(newCard, true);
        _cardsHandView.CardAdd(newCard);
    }
}
