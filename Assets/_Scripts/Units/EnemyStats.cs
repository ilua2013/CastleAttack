using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class EnemyStats
{
    [SerializeField] private ModifyStats _stats;

    public int GetModifyDamage(int baseDamage, int castle)
    {
        if (_stats.Castle <= castle)
            return _stats.ExtraDamage + baseDamage;

        return baseDamage;
    }

    public int GetModifyHealth(int baseHealth, int castle)
    {
        if (_stats.Castle <= castle)
            return _stats.ExtraHealth + baseHealth;

        return baseHealth;
    }

    [Serializable]
    public class ModifyStats
    {
        [SerializeField] private int _castle;
        [SerializeField] private int _extraDamage;
        [SerializeField] private int _extraHealth;

        public int Castle => _castle;
        public int ExtraDamage => _extraDamage;
        public int ExtraHealth => _extraHealth;
    }
}
