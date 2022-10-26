using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpellSpawners : SpellSpawner
{
    protected override bool TryApplySpell(Card card, Vector3 place)
    {
        if (card.Description is MeteorSpellCardDescription)
        {
            MeteorSpellCardDescription description = card.Description as MeteorSpellCardDescription;
            MeteorSpell spell = Instantiate(description.SpellPrefab, place, Quaternion.identity);

            spell.Cast();
            spell.Init(description.DamagePerSecond);

            return true;
        }

        return false;
    }
}
