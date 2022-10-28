using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : Card
{
    [SerializeField] private AoESpell _spellPrefab;

    public AoESpell SpellPrefab => _spellPrefab;
}
