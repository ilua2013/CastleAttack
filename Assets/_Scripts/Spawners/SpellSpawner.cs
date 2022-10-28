using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour, ICardApplicable
{
    public bool TryApply(Card card, Vector3 place)
    {
        if (card is SpellCard)
        {
            SpellCard spellCard = card as SpellCard;
            AoESpell spell = Instantiate(spellCard.SpellPrefab, place, Quaternion.identity);

            spell.Cast();
            return true;
        }
        return false;
    }
}
