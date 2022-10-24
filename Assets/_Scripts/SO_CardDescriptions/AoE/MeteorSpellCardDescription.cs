using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell Card", menuName = "Cards/Spell Cards/New MeteorSpell Card")]
public class MeteorSpellCardDescription : AoESpellCardDescription
{
    [SerializeField] private MeteorSpell _spellPrefab;
    [SerializeField] private float _damagePerSecond;

    public MeteorSpell SpellPrefab => _spellPrefab;
    public float DamagePerSecond => _damagePerSecond;
}
