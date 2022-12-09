using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour, ICardApplicable
{
    private Cell _cell;
    private BattleSystem _battleSystem;

    public UnitFriend Spawned { get; private set; }

    public Vector3 SpawnPoint => transform.position;

    public Cell Cell => _cell;

    public event Action<Vector3, Spell> Cast;

    private void Awake()
    {
        _cell = GetComponent<Cell>();
        _battleSystem = FindObjectOfType<BattleSystem>();
    }

    public bool TryApplyFriend(Card card, Vector3 place)
    {
        if (card is SpellCard spellCard)
        {
            Spell spell = Instantiate(spellCard.SpellPrefab, transform.position, Quaternion.identity);

            spell.Cast(_cell, card.CardSave, _battleSystem);
            Cast?.Invoke(place, spell);
            return true;
        }
        return false;
    }

    public bool CanApply(Card card)
    {
        return card is SpellCard;
    }
}
