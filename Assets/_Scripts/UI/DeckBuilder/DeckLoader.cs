using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckLoader : MonoBehaviour
{
    private CommonDecks _deck;
    private List<CardInDeckView> _views;
    private List<Card> _cards = new List<Card>();

    private void Awake()
    {
        _deck = FindObjectOfType<CommonDecks>();
        _views = GetComponentsInChildren<CardInDeckView>().ToList();
    }

    private void Start()
    {
        _cards = new List<Card>(_deck.Cards);

        FillCards(false);
    }

    private void FillCards(bool smooth)
    {
        for (int i = 0; i < _views.Count; i++)
            _views[i].Clear();

        for (int i = 0; i < _cards.Count; i++)
        {
            for (int j = 0; j < _views.Count; j++)
            {
                if (_cards[i].Name == _views[j].CardName)
                    _views[j].FillCard(_cards[i], smooth);
            }
        }
    }
}
