using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static EnemySpawner;
using Random = System.Random;

[CreateAssetMenu(fileName = "LevelEnemies", menuName = "Configs/Level/Enemies")]
public class LevelEnemiesData : ScriptableObject
{
    public List<EnemiesData> EnemiesData = new List<EnemiesData>();

    public void SetWaves(List<Wave> waves, int level)
    {
        level = Mathf.Max(level, 0);

        EnemiesData data = EnemiesData.FirstOrDefault((item) => item.Level == level);

        if (data == null)
            throw new ArgumentException($"Level {level} does not exists");

        data.GenerateWaves();

        for (int i = 0; i < data.WavesCount; i++)
            waves.Add(new Wave(data.Waves[i].UnitEnemies));
    }
}

[Serializable]
public class EnemiesData
{
    public int Level;
    public CellSize CellSize;
    public int WavesCount;
    public EnemyType[] EnemyTypes;

    [HideInInspector] private List<Wave> _waves = new List<Wave>();
    [HideInInspector] private List<UnitEnemy> _enemies = new List<UnitEnemy>();

    public List<Wave> Waves => _waves;

    public EnemiesData(int level)
    {
        Level = level;
    }
    
    public void GenerateWaves()
    {
        CollectEnemies();
        Shuffle();
        CollectWaves();
    }

    private void CollectWaves()
    {
        _waves = new List<Wave>();

        var splitEnemies = _enemies.ToArray().Split(WavesCount);

        foreach (var enemies in splitEnemies)
            _waves.Add(new Wave(enemies.ToArray()));
    }

    private void CollectEnemies()
    {
        _enemies = new List<UnitEnemy>();

        foreach (EnemyType type in EnemyTypes)
            _enemies.AddRange(type.GetEnemies());
    }

    private void Shuffle()
    {
        Random random = new Random();

        for (int i = _enemies.Count - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);

            UnitEnemy temp = _enemies[j];
            _enemies[j] = _enemies[i];
            _enemies[i] = temp;
        }
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

[Serializable]
public class CellSize
{
    public int Height;
    public int Weight;

    public CellSize(int height, int weight)
    {
        Height = height;
        Weight = weight;
    }

    public static bool CheckEqualSize(CellSize one, CellSize two)
    {
        if (one.Height == two.Height && one.Weight == two.Weight)
            return true;
        else
            return false;
    }
}
