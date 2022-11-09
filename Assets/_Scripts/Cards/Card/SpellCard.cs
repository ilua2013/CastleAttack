using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCard : Card
{
    [SerializeField] private Spell _spellPrefab;

    public Spell SpellPrefab => _spellPrefab;
}
