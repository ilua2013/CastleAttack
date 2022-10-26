using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellSpawner : MonoBehaviour, ICardApplicable
{
    public bool TryApply(Card card, Vector3 place)
    {
        return TryApplySpell(card, place);
    }

    protected abstract bool TryApplySpell(Card card, Vector3 place);
}
