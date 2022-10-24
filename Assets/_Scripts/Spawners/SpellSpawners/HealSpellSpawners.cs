using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpellSpawners : SpellSpawner
{
    protected override bool TryApplySpell(CardDescription card, Vector3 place)
    {
        if (card is HealSpellCardDescription)
        {
            HealSpellCardDescription description = card as HealSpellCardDescription;
            HealSpell spell = Instantiate(description.SpellPrefab, place, Quaternion.identity);

            spell.Cast();
            spell.Init(description.RecoveryPerSecond);

            return true;
        }

        return false;
    }
}
