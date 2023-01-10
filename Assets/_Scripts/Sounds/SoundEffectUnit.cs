using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitExperience))]
public class SoundEffectUnit : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer _settings;

    private IUnit _unit;
    private UnitExperience _experience;

    private void Awake()
    {
        _unit = GetComponent<IUnit>();
        _experience = GetComponent<UnitExperience>();
    }

    private void OnEnable()
    {
        _unit.Fighter.Attacked += OnAttack;
        _unit.Mover.StartedMove += OnMoved;
        _experience.LevelUp += OnLevelUp;
    }

    private void OnDisable()
    {
        _unit.Fighter.Attacked -= OnAttack;
        _unit.Mover.StartedMove -= OnMoved;
        _experience.LevelUp -= OnLevelUp;
    }

    private void OnAttack()
    {
        _settings.Play(SoundEffectType.Attack);
    }

    private void OnLevelUp(int level)
    {
        _settings.Play(SoundEffectType.LevelUp);
    }

    private void OnMoved()
    {
        _settings.Play(SoundEffectType.StartStep);

        StartCoroutine(PlayWithDelay(0.5f, () =>
        _settings.Play(SoundEffectType.Steps)));
    }

    private IEnumerator PlayWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();
    }
}
