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

    public void Create(Card card)
    {
        if (card is UnitCard unitCard)
            CreateUnit(unitCard);
        else if (card is SpellCard spellCard)
            CreateSpell(spellCard);
    }

    private void OnCardStageUp(UnitCard card)
    {
        CreateUnit(card.NextStage);
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
