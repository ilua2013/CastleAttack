using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DeckCounter))]
public class DeckCounterView : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private Image _image;
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

        if (_deck.Cards.Count <= 0)
            _image.color = new Color32(255, 255, 255, 125);
        else
            _image.color = new Color32(255, 255, 255, 255);
    }
}
