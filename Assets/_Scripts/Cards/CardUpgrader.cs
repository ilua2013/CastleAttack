using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardUpgrader : MonoBehaviour
{
    [SerializeField] private CardsHand _cardsHand;
    [SerializeField] private CardsHandView _cardsHandView;

    private List<UnitCard> _cards = new List<UnitCard>();
    private CardsSelection _cardsSelection;

    private void Awake()
    {
        _cards = GetComponentsInChildren<UnitCard>().ToList();
        _cardsSelection = FindObjectOfType<CardsSelection>(true);
    }

    private void OnEnable()
    {
        _cardsSelection.CardSelected += OnCardSelect;

        foreach (UnitCard card in _cards)
        {
            card.StageUp += OnCardStageUp;
            card.CameBack += OnCameBack;
        }
    }

    private void OnDisable()
    {
        _cardsSelection.CardSelected -= OnCardSelect;

        foreach (UnitCard card in _cards)
        {
            card.StageUp -= OnCardStageUp;
            card.CameBack -= OnCameBack;
        }
    }

    private void OnCardSelect(Card card)
    {
        if (card is UnitCard unitCard)
        {
            unitCard.transform.SetParent(_cardsHand.transform);

            unitCard.StageUp += OnCardStageUp;
            unitCard.CameBack += OnCameBack;

            _cards.Add(unitCard);
            _cardsHand.CardAdd(unitCard);
            _cardsHandView.CardAdd(unitCard);
        }
        else if (card is SpellCard spellCard)
        {
            spellCard.transform.SetParent(_cardsHand.transform);
            _cardsHand.CardAdd(spellCard);
            _cardsHandView.CardAdd(spellCard);
        }
    }

    private void OnCardStageUp(UnitCard card)
    {
        foreach (var other in _cards)
        {
            if (other.UnitPrefab == card.NextStage.UnitPrefab)
            {
                if (other.gameObject.activeInHierarchy)
                {
                    other.Merge();
                    return;
                }
            }
        }

        UnitCard newCard = Instantiate(card.NextStage, _cardsHand.transform);

        newCard.StageUp += OnCardStageUp;
        newCard.CameBack += OnCameBack;

        _cards.Add(newCard);
        _cardsHand.CardAdd(newCard);
        _cardsHandView.CardAdd(newCard);
    }

    private void OnCameBack(UnitCard card)
    {
        _cardsHand.CardComeBack(card);
        _cardsHandView.CardComeBack(card);
    }
}
