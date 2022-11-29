using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Spell))]
public class SoundEffectSpell : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer _settings;

    private Spell _spell;

    private void Awake()
    {
        _spell = GetComponent<Spell>();
    }

    private void OnEnable()
    {
        _spell.WasCast += OnCast;
    }

    private void OnDisable()
    {
        _spell.WasCast -= OnCast;
    }

    private void OnCast()
    {
        StartCoroutine(PlayWithDelay(0.2f, () =>
        _settings.Play(SoundEffectType.Spell)));
    }

    private IEnumerator PlayWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();
    }
}
