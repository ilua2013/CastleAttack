using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<UnitSpawner> _cellsEnemySpawner;
    [SerializeField] private List<UnitStep> _enemyUnitsPrefab;
    [SerializeField] private FightSystem _fightSystem;

    private void OnEnable()
    {
        _fightSystem.StepFinished += EnemySpawn;
    }
    private void OnDisable()
    {
        _fightSystem.StepFinished -= EnemySpawn;
    }

    private void EnemySpawn()
    {
        _cellsEnemySpawner[0].TryApplyEnemy(_enemyUnitsPrefab[0]);
    }

}
