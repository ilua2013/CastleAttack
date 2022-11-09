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

    private void Awake()
    {
        _cards = GetComponentsInChildren<UnitCard>().ToList();
    }

    private void OnEnable()
    {
        foreach (UnitCard card in _cards)
        {
            card.StageUp += OnCardStageUp;
            card.CameBack += OnCameBack;
        }
    }

    private void OnDisable()
    {
        foreach (UnitCard card in _cards)
        {
            card.StageUp -= OnCardStageUp;
            card.CameBack -= OnCameBack;
        }
    }

    private void OnCardStageUp(UnitCard card)
    {
        UnitCard unitCard = Instantiate(card.NextStage, _cardsHand.transform) as UnitCard;

        unitCard.StageUp += OnCardStageUp;
        unitCard.CameBack += OnCameBack;

        _cards.Add(unitCard);
        _cardsHand.CardAdd(unitCard);
        _cardsHandView.CardAdd(unitCard);
    }

    private void OnCameBack(UnitCard card)
    {
        _cardsHand.CardComeBack(card);
        _cardsHandView.CardComeBack(card);
    }
}
