using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardPanel : MonoBehaviour
{
    [SerializeField] private CardRewardView _containerPrefab;
    [SerializeField] private Transform _panel;

    private List<Card> _cards = new List<Card>();

    public void ShowCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            Card newCard = Instantiate(card);
            CardRewardView view = Instantiate(_containerPrefab, _panel);

            view.Fill(newCard, card.CardSave.RewardAmount);

            _cards.Add(newCard);
        }
    }
}
