using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SoundEffect
{
    public SoundEffectType SoundEffectType;
    public List<AudioClip> AudioClips;
}

public enum SoundEffectType
{
    LevelUp,
    Attack,
    Steps,
    Death,
    Meteor,
    Heal,
    StartFight,
    Spawn,
}

[Serializable]
public class SoundEffectPlayer
{
    [SerializeField] private List<SoundEffect> _soundEffects;

    public void Play(SoundEffectType effectType, Vector3 point)
    {
        SoundEffect effect = _soundEffects.Find(item => item.SoundEffectType == effectType);
        int index = UnityEngine.Random.Range(0, effect.AudioClips.Count);

        AudioSource.PlayClipAtPoint(effect.AudioClips[index], point, 0.5f);
    }
}
