using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpellSpawners : SpellSpawner
{
    protected override bool TryApplySpell(Card card, Vector3 place)
    {
        if (card.Description is HealSpellCardDescription)
        {
            HealSpellCardDescription description = card.Description as HealSpellCardDescription;
            HealSpell spell = Instantiate(description.SpellPrefab, place, Quaternion.identity);

            spell.Cast();
            spell.Init(description.RecoveryPerSecond);

            return true;
        }

        return false;
    }
}
