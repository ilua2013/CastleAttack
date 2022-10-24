using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpellSpawners : SpellSpawner
{
    protected override bool TryApplySpell(CardDescription card, Vector3 place)
    {
        if (card is MeteorSpellCardDescription)
        {
            MeteorSpellCardDescription description = card as MeteorSpellCardDescription;
            MeteorSpell spell = Instantiate(description.SpellPrefab, place, Quaternion.identity);

            spell.Cast();
            spell.Init(description.DamagePerSecond);

            return true;
        }

        return false;
    }
}
