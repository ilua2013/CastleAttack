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
    private CardsSelection _cardsSelection;

    public event Action<UnitCard, UnitCard> CardUp;

    private void Awake()
    {
        _unitCards = GetComponentsInChildren<UnitCard>().ToList();
        _cardsSelection = FindObjectOfType<CardsSelection>(true);
    }

    private void OnEnable()
    {
        _cardsSelection.CardSelected += OnCardSelect;

        foreach (UnitCard card in _unitCards)
        {
            card.StageUp += OnCardStageUp;
            card.CameBack += OnCameBack;
        }
    }

    private void OnDisable()
    {
        _cardsSelection.CardSelected -= OnCardSelect;

        foreach (UnitCard card in _unitCards)
        {
            card.StageUp -= OnCardStageUp;
            card.CameBack -= OnCameBack;
        }
    }

    private void OnCardSelect(Card card)
    {
        if (card is UnitCard unitCard)
            CreateUnit(unitCard);
        else if (card is SpellCard spellCard)
            CreateSpell(spellCard);
    }

    private void OnCardStageUp(UnitCard card)
    {
        CreateUnit(card.NextStage);
        CardUp?.Invoke(card, card.NextStage);
    }

    private void OnCameBack(UnitCard card)
    {
        _cardsHand.CardComeBack(card);
        _cardsHandView.CardComeBack(card);

    }

    private void CreateUnit(UnitCard card)
    {
        foreach (UnitCard unitCard in _unitCards)
        {
            if (unitCard.UnitPrefab == card.UnitPrefab)
            {
                if (unitCard.gameObject.activeInHierarchy)
                {
                    unitCard.Merge();
                    return;
                }
            }
        }

        UnitCard newCard = Instantiate(card, _cardsHand.transform);
        newCard.gameObject.SetActive(true);

        newCard.StageUp += OnCardStageUp;
        newCard.CameBack += OnCameBack;

        _unitCards.Add(newCard);
        _cardsHand.CardAdd(newCard);
        _cardsHandView.CardAdd(newCard);
    }

    private void CreateSpell(SpellCard card)
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

        SpellCard newCard = Instantiate(card, _cardsHand.transform);
        newCard.gameObject.SetActive(true);

        _spellCards.Add(newCard);
        _cardsHand.CardAdd(newCard);
        _cardsHandView.CardAdd(newCard);
    }
}
