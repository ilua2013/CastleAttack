using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialEffects : MonoBehaviour
{   
    [SerializeField] private ParticleSystem[] _particleSystemSelectStars;    
    [SerializeField] private ParticleSystem[] _particleDrawCard;
    [SerializeField] private ParticleSystem[] _particleStartButton;
    [SerializeField] private ParticleSystem[] _particleBox;
    [SerializeField] private GameObject _auraCard;

    private void Start()
    {       
        ParticleStop(_particleDrawCard);
        ParticleStop(_particleStartButton);
        ParticleStop(_particleSystemSelectStars);
        ParticleStop(_particleBox);
    }

    public void EffectOneTwo()
    {
        ParticlePlay(_particleSystemSelectStars);        
        ParticlePlay(_particleDrawCard);       
    }

    public void EffectTwoThree()
    {
        ParticlePlay(_particleSystemSelectStars);       
        ParticleStop(_particleDrawCard);       
        ParticlePlay(_particleStartButton);
    }

    public void EffectThreeFour()
    {
        ParticleStop(_particleStartButton);
        _auraCard.SetActive(false);
        ParticlePlay(_particleSystemSelectStars);
    }

    public void EffectFourFive()
    {
        ParticlePlay(_particleSystemSelectStars);
    }

    public void ParticleBox()
    {
        ParticlePlay(_particleBox);
    }

    private void ParticleStop(ParticleSystem[] particleSystems)
    {
        foreach (var particle in particleSystems)
        {
            particle.Stop();
        }
    }

    private void ParticlePlay(ParticleSystem[] particleSystems)
    {
        foreach (var particle in particleSystems)
        {
            particle.Play();
        }
    }
}
