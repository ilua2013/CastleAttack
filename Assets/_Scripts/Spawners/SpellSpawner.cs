using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour, ICardApplicable
{
    public UnitFriend Spawned { get; private set; }

    public Vector3 SpawnPoint => transform.position;

    public event Action<Vector3, Spell> Cast;

    public bool TryApplyFriend(Card card, Vector3 place)
    {
        if (card is SpellCard spellCard)
        {
            Spell spell = Instantiate(spellCard.SpellPrefab, place, Quaternion.identity);

            spell.Cast();
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
