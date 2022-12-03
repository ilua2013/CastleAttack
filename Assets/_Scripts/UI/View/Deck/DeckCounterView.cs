using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(DeckCounter))]
public class DeckCounterView : MonoBehaviour
{
    [SerializeField] private TMP_Text _countText;

    private DeckCounter _deckCounter;

    private void Awake()
    {
        _deckCounter = GetComponent<DeckCounter>();
    }

    private void OnEnable()
    {
        _deckCounter.Decreased += OnDecreased;
    }

    private void OnDisable()
    {
        _deckCounter.Decreased -= OnDecreased;
    }

    private void OnDecreased(int count)
    {
        _countText.text = count.ToString();
    }
}
