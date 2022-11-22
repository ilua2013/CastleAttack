using UnityEngine;

public interface ICardApplicable
{
    Cell Cell { get; }
    Vector3 SpawnPoint { get; }
    UnitFriend Spawned { get; }
    bool CanApply(Card card);
    bool TryApplyFriend(Card card, Vector3 place);
}
