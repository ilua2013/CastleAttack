using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectEnemy : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer _settings;

    private IUnit _unit;

    private void Awake()
    {
        _unit = GetComponent<IUnit>();
    }

    private void OnEnable()
    {
        _unit.Fighter.Attacked += OnAttack;
        _unit.Mover.Moved += OnMoved;
    }

    private void OnDisable()
    {
        _unit.Fighter.Attacked -= OnAttack;
        _unit.Mover.Moved -= OnMoved;
    }

    private void OnAttack()
    {
        _settings.Play(SoundEffectType.Attack);
    }

    private void OnMoved()
    {
        StartCoroutine(PlayWithDelay(0.5f, () =>
        _settings.Play(SoundEffectType.Steps)));
    }

    private IEnumerator PlayWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();
    }
}
