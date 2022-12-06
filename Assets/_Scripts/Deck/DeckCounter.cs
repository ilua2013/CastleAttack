using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckCounter : MonoBehaviour
{
    private int _count;
    private CommonDeck _deck;

    public bool CanTakeCard => true;

    public event Action<int> Decreased;

    private void Awake()
    {
        _deck = FindObjectOfType<CommonDeck>();
    }

    private void Start()
    {
        //_count = _deck.
    }
}
