using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCardView : MonoBehaviour
{
    private UnitEnemyType _typeId;

    public void Init(UnitEnemyType typeId)
    {
        _typeId = typeId;
    }
}
