using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSpawner : MonoBehaviour, ICardApplicable
{
    public bool TryApply(CardDescription card, Vector3 place)
    {
        if (card is AoESpellCardDescription)
        {
            AoESpellCardDescription description = card as AoESpellCardDescription;
            AoESpell spell = Instantiate(description.SpellPrefab, place, Quaternion.identity);

            spell.Cast();

            return true;
        }

        return false;
    }
}
