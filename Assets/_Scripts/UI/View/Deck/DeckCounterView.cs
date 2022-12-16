using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DeckCounter))]
public class DeckCounterView : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private CombatDeck _deck;

    private void OnValidate()
    {
        if (_deck == null)
            _deck = FindObjectOfType<CombatDeck>();
    }

    private void OnEnable()
    {
        _deck.CardTaken += OnDecreased;
        _deck.CardReturned += OnDecreased;
    }

    private void OnDisable()
    {
        _deck.CardTaken -= OnDecreased;
        _deck.CardReturned -= OnDecreased;
    }

    private void OnDecreased()
    {
        _countText.text = _deck.Cards.Count.ToString();
    }
}
