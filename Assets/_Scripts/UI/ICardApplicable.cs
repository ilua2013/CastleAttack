using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardApplicable
{
    bool TryApplyFriend(Card card, Vector3 place);
}
