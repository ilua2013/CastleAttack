using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private SpellSpawner[] _spellSpawners;

    enum State
    {
        Meteor,
        Heal,
    }

    private void Awake()
    {
        _spellSpawners = FindObjectsOfType<SpellSpawner>();
    }

    private void OnEnable()
    {
        foreach (SpellSpawner spawner in _spellSpawners)
            spawner.Cast += OnSpellCast;
    }

    private void OnDisable()
    {
        foreach (SpellSpawner spawner in _spellSpawners)
            spawner.Cast -= OnSpellCast;
    }

    private void OnSpellCast(Vector3 place, Spell spell)
    {
        if (spell is MeteorSpell)
            _animator.SetTrigger(State.Meteor.ToString());
        else if (spell is HealSpell)
            _animator.SetTrigger(State.Heal.ToString());
    }
}
