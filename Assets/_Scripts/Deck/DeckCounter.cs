using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckCounter : MonoBehaviour
{
    [SerializeField] private CombatDeck _deck;

    public bool CanTakeCard => !_deck.IsEmpty;

    public event Action<int> Decreased;

    private void OnValidate()
    {
        if (_deck == null)
            _deck = FindObjectOfType<CombatDeck>();
    }
}
