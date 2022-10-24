using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardApplicable
{
    bool TryApply(CardDescription card, Vector3 place);
}
