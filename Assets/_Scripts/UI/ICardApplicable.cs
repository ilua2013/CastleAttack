using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardApplicable
{
    bool TryApply(Card card, Vector3 place);
}
