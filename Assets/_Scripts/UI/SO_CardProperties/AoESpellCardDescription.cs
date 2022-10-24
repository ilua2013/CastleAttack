using UnityEngine;

[CreateAssetMenu(fileName = "Spell Card", menuName = "Cards/Spell Cards/New AoESpell Card")]
public class AoESpellCardDescription : CardDescription
{
    [SerializeField] private AoESpell _spellPrefab;

    public AoESpell SpellPrefab => _spellPrefab;
}
