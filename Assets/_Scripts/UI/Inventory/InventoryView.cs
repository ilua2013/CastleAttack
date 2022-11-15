using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject[] _childs;

    private Deck _deck;
    private List<InventoryCardView> _inventoryViews;

    private void Awake()
    {
        _deck = FindObjectOfType<Deck>();
        _inventoryViews = GetComponentsInChildren<InventoryCardView>(true).ToList();
    }

    private void OnEnable()
    {
        _openButton.onClick.AddListener(Show);
        _closeButton.onClick.AddListener(Hide);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(Show);
        _closeButton.onClick.RemoveListener(Hide);
    }

    private void Show()
    {
        foreach (var child in _childs)
            child.SetActive(true);

        for (int i = 0; i < _deck.Cards.Count && i < _inventoryViews.Count; i++)
            _inventoryViews[i].FillCard(_deck.Cards[i]);
    }

    private void Hide()
    {
        foreach (var child in _childs)
            child.SetActive(false);
    }
}
