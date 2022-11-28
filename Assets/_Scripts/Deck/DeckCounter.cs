using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckCounter : MonoBehaviour
{
    [SerializeField] private int _count;

    private int _initialCount;
    private LevelSystem _levelSystem;
    private CardsSelection _cardsSelection;

    public bool CanTakeCard => _count > 0;

    public event Action<int> Decreased;

    private void Awake()
    {
        _levelSystem = FindObjectOfType<LevelSystem>(true);
        _cardsSelection = FindObjectOfType<CardsSelection>(true);
        _initialCount = _count;
    }

    private void OnEnable()
    {
        _levelSystem.WaveFinished += OnWaveFinished;
        _cardsSelection.CardSelected += OnCardSelected;
    }

    private void OnDisable()
    {
        _levelSystem.WaveFinished -= OnWaveFinished;
        _cardsSelection.CardSelected -= OnCardSelected;
    }

    private void OnWaveFinished()
    {
        _count = _initialCount;
    }

    private void OnCardSelected(Card card)
    {
        _count--;
        Decreased?.Invoke(_count);
    }
}
