using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell Card", menuName = "Cards/Spell Cards/New HealSpell Card")]
public class HealSpellCardDescription : AoESpellCardDescription
{
    [SerializeField] private HealSpell _spellPrefab;
    [SerializeField] private float _recoveryPerSecond;

    public HealSpell SpellPrefab => _spellPrefab;
    public float RecoveryPerSecond => _recoveryPerSecond;
}
