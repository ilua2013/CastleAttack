using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXActivator : MonoBehaviour
{
    [SerializeField] private ParticleSystem _meteor;
    [SerializeField] private ParticleSystem _heal;

    public void OnMeteorCast()
    {
        _meteor.Play();
    }

    public void OnHealCast()
    {
        _heal.Play();
    }
}
