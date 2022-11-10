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
