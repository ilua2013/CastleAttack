using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnemySpawner;

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

    public void SetWaves(List<Wave> waves, int level)
    {
        EnemiesData data = EnemiesData.FirstOrDefault((item) => item.Level == level);

        if (data == null)
            throw new ArgumentException($"Level {level} does not exists");

        for (int i = 0; i < data.Waves; i++)
            waves.Add(new Wave(data.GetRandomEnemies()));
    }
}

[Serializable]
public class EnemiesData
{
    public int Level;
    public int Waves;
    public EnemyType[] EnemyTypes;

    private Dictionary<int, UnitEnemy[]> _enemies = new Dictionary<int, UnitEnemy[]>();

    public EnemiesData(int level)
    {
        Level = level;
    }

    public UnitEnemy[] GetRandomEnemies()
    {
        List<UnitEnemy> enemies = new List<UnitEnemy>();

        for (int i = 0; i < EnemyTypes.Length; i++)
            enemies.AddRange(EnemyTypes[i].GetEnemies());

        for (int i = 0; i < enemies.Count; i++)
        {
            //_enemies.Add()
        }

        return enemies.ToArray();
    }
}

[Serializable]
public class EnemyType
{
    public UnitEnemy EnemyPrefab;
    public int Amount;

    public UnitEnemy[] GetEnemies()
    {
        UnitEnemy[] enemies = new UnitEnemy[Amount];

        for (int i = 0; i < Amount; i++)
            enemies[i] = EnemyPrefab;

        return enemies;
    }
}
