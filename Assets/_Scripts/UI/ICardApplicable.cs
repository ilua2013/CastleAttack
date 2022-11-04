using UnityEngine;

public interface ICardApplicable
{
    UnitFriend Spawned { get; }
    bool TryApplyFriend(Card card, Vector3 place);
}
