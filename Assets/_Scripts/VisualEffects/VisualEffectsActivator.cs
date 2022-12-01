using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum VFX
{
    Spawn,
    Attack,
}

[Serializable]
public class VisualEffect
{
    [SerializeField] private ParticleSystem _prefab;
    [SerializeField] private VFX _type;

    public ParticleSystem Prefab => _prefab;
    public VFX Type => _type;
}

public class VisualEffectsActivator : MonoBehaviour
{
    [SerializeField] private UnitFriend _unit;
    [SerializeField] private List<VisualEffect> _visualEffects;

    private Dictionary<VFX, ParticleSystem> _effects = new Dictionary<VFX, ParticleSystem>();

    private void OnEnable()
    {
        _unit.Inited += OnInitialized;
        _unit.Fighter.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        _unit.Inited -= OnInitialized;
        _unit.Fighter.Damaged -= OnDamaged;
    }

    private void OnInitialized()
    {
        foreach (VisualEffect effect in _visualEffects)
        {
            if (effect.Type == VFX.Spawn)
                PlayEffect(effect.Prefab, VFX.Spawn);
        }
    }

    private void OnDamaged(int damage)
    {
        foreach (VisualEffect effect in _visualEffects)
        {
            if (effect.Type == VFX.Attack)
                PlayEffect(effect.Prefab, VFX.Attack);
        }
    }

    private void PlayEffect(ParticleSystem prefab, VFX type)
    {
        if (_effects.ContainsKey(type))
        {
            _effects[type].Play();
            return;
        }

        ParticleSystem effect = Instantiate(prefab, transform.position, Quaternion.identity);
        _effects.Add(type, effect);

        effect.transform.position = transform.position;
        effect.Play();
    }
}
