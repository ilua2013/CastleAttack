using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour, ICardApplicable
{
    [SerializeField] private Transform _spawnPoint;

    public bool TryApply(CardDescription card, Vector3 place)
    {
        if (card is UnitCardDescription)
        {
            UnitCardDescription description = card as UnitCardDescription;

            Instantiate(description.UnitTemplate, _spawnPoint.position, _spawnPoint.rotation);
            return true;
        }

        return false;
    }
}
