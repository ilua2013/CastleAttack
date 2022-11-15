using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class InventoryCardsMover : MonoBehaviour
{
    private Inventory _inventory;
    private List<CardMovement> _movements = new List<CardMovement>();

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        foreach (Card card in _inventory.CommonDeck)
            if (card.TryGetComponent(out CardMovement movement))
            {
                movement.Init(transform);
                _movements.Add(movement);
            }

        foreach (Card card in _inventory.CombatDeck)
            if (card.TryGetComponent(out CardMovement movement))
            {
                movement.Init(transform);
                _movements.Add(movement);
            }
    }

    private void Update()
    {
        foreach (CardMovement movement in _movements)
            movement.Move();
    }
}
