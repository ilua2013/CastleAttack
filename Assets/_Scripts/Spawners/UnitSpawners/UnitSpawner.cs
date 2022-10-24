using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitSpawner : MonoBehaviour, ICardApplicable
{
    [SerializeField] private Transform _spawnPoint;

    public Transform SpawnPoint => _spawnPoint;

    public bool TryApply(CardDescription card, Vector3 place)
    {
        return TryApplySpell(card, place);
    }

    protected abstract bool TryApplySpell(CardDescription card, Vector3 place);
}
