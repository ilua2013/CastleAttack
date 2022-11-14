using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Button _openButton;

    private Deck _deck;
    private InventoryCardView[] _cards;

    private void Awake()
    {
        _deck = FindObjectOfType<Deck>();
        _cards = GetComponentsInChildren<InventoryCardView>();
    }

    private void OnEnable()
    {
        _openButton.onClick.AddListener(Show);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(Show);
    }

    private void Show()
    {

    }
}
