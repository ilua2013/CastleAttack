using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellSpawner : MonoBehaviour, ICardApplicable
{
    public bool TryApply(CardDescription card, Vector3 place)
    {
        return TryApplySpell(card, place);
    }

    protected abstract bool TryApplySpell(CardDescription card, Vector3 place);
}
