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
        _cellsEnemySpawner[Random.Range(0, _cellsEnemySpawner.Count)].TryApplyEnemy(_enemyUnitsPrefab[Random.Range(0, _enemyUnitsPrefab.Count)]);
    }

}
