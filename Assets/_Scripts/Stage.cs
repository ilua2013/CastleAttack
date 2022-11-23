using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private StageNumber _stage;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private CellSpawner _cellSpawner;

    public StageNumber StageNumber => _stage;
    public EnemySpawner EnemySpawner => _enemySpawner;
    public CellSpawner CellSpawner => _cellSpawner;

    private void OnValidate()
    {
        _enemySpawner = GetComponentInChildren<EnemySpawner>();
        _cellSpawner = GetComponentInChildren<CellSpawner>();
    }
}

