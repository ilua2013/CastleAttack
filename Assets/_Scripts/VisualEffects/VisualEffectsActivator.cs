using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum VFX
{
    Spawn,
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

    private void OnEnable()
    {
        _unit.Inited += OnInitialized;
    }

    private void OnDisable()
    {
        _unit.Inited -= OnInitialized;
    }

    private void OnInitialized()
    {
        foreach (VisualEffect effect in _visualEffects)
        {
            if (effect.Type == VFX.Spawn)
                PlayEffect(effect.Prefab);
        }
    }

    private void PlayEffect(ParticleSystem prefab)
    {
        ParticleSystem effect = Instantiate(prefab, transform.position, Quaternion.identity);
        effect.Play();
    }
}
