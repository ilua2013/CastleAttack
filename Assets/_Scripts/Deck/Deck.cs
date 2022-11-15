using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> _deckPrefabs;

    private List<Card> _cards = new List<Card>();

    public List<Card> Cards => _cards;
    public virtual string SaveKey { get; }

    private void Awake()
    {
        if (Saves.HasKey(SaveKey))
            _deckPrefabs = Saves.GetDeck(SaveKey).ToList();

        CreateDeck();
    }

    private void CreateDeck()
    {
        foreach (Card card in _deckPrefabs)
        {
            Card newCard = Instantiate(card);
            newCard.Activate(true);

            _cards.Add(newCard);
        }
    }
}
