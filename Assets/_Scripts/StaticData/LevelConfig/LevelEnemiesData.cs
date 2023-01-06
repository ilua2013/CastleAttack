using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelEnemies", menuName = "Configs/Level/Enemies")]
public class LevelEnemiesData : ScriptableObject
{
    [Header("Clear config")]
    public bool ClearAll;

    [Space]
    public int LevelCount;
    public List<EnemiesData> EnemiesData = new List<EnemiesData>();

    private void OnValidate()
    {
        if (ClearAll)
        {
            EnemiesData.Clear();

            for (int i = 0; i < LevelCount; i++)
                EnemiesData.Add(new EnemiesData(i));

            ClearAll = false;
        }
    }
}

[Serializable]
public class EnemiesData
{
    public int Level;
    public int Waves;
    public EnemyData[] Enemies;

    public EnemiesData(int level)
    {
        Level = level;
    }
}

[Serializable]
public class EnemyData
{
    public UnitEnemy EnemyPrefab;
    public int Amount;
}
