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
    Spell,
    StartFight,
    Spawn,
    Panel,
    BackgroundMenu,
    BackgroundBattle,
    CardClick,
    CardComeBack,
}

[Serializable]
public class SoundEffectPlayer
{
    [SerializeField] private List<SoundEffect> _soundEffects;

    public void Play(SoundEffectType effectType)
    {
        SoundEffect effect = _soundEffects.Find(item => item.SoundEffectType == effectType);
        int index = UnityEngine.Random.Range(0, effect.AudioClips.Count);

        AudioSource.PlayClipAtPoint(effect.AudioClips[index], SoundSourceObject.Instance.Position, 1);
    }
}
