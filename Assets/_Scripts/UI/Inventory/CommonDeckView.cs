using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommonDeckView : MonoBehaviour
{
    [SerializeField] private PanelController _panelController;
    [SerializeField] private Inventory _inventory;
    
    private List<InventoryCardView> _inventoryViews;

    private void Awake()
    {
        _panelController = GetComponent<PanelController>();
        _inventory = GetComponent<Inventory>();
        _inventoryViews = GetComponentsInChildren<InventoryCardView>(true).ToList();
    }

    private void OnEnable()
    {
        _panelController.Opened += OnPanelOpen;
        _panelController.Closed -= OnClosePanel;
    }

    private void OnDisable()
    {
        _panelController.Opened += OnPanelOpen;
        _panelController.Closed -= OnClosePanel;
    }

    private void OnPanelOpen()
    {
        for (int i = 0; i < _inventory.CommonDeck.Count && i < _inventoryViews.Count; i++)
            _inventoryViews[i].FillCard(_inventory.CommonDeck[i]);
    }

    private void OnClosePanel()
    {

    }
}
