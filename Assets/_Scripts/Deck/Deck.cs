using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> _cards;

    private List<Card> _cardObjects = new List<Card>();

    public IEnumerable<Card> Cards => _cardObjects;

    private void Awake()
    {
        foreach (Card card in _cards)
        {
            Card newCard = Instantiate(card, transform);
            newCard.gameObject.SetActive(false);

            _cardObjects.Add(newCard);
        }
    }

    public Card[] ShowRandomCards(int count)
    {
        if (count > _cardObjects.Count)
            throw new ArgumentOutOfRangeException($"{count} is more then cards in the Deck");

        List<Card> result = new List<Card>(count);

        for (int i = 0; i < count; i++)
        {
            Card card = _cardObjects[Random.Range(0, _cardObjects.Count)];

            if (result.Contains(card))
            {
                i--;
                continue;
            }

            result.Add(card);
        }

        return result.ToArray();
    }

    public void ReturnCards(Card[] cards)
    {
        foreach (Card card in cards)
        {
            card.transform.SetParent(transform);
            card.gameObject.SetActive(false);
        }
    }
}
