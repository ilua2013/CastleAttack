using UnityEngine;

public interface ICardApplicable
{
    UnitFriend Spawned { get; }
    bool CanApply(Card card);
    bool TryApplyFriend(Card card, Vector3 place);
}
