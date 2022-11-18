using UnityEngine;

public interface ICardApplicable
{
    Vector3 SpawnPoint { get; }
    UnitFriend Spawned { get; }
    bool CanApply(Card card);
    bool TryApplyFriend(Card card, Vector3 place);
}
