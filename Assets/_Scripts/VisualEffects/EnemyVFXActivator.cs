using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitEnemy))]
public class EnemyVFXActivator : MonoBehaviour
{
    [SerializeField] private ParticleSystem _dieVfx;

    private UnitEnemy _unit;

    private void Awake()
    {
        _unit = GetComponent<UnitEnemy>();
    }

    private void OnEnable()
    {
        _unit.Fighter.Died += OnDie;
    }

    private void OnDisable()
    {
        _unit.Fighter.Died -= OnDie;
    }

    private void OnDie()
    {
        _dieVfx.Play();
    }
}
