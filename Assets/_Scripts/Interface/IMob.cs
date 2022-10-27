using TypesMobs;
using UnityEngine;
using UnityEngine.Events;

public interface IMob : IDamageable
{
    public Vector3 TransformPosition { get; }
    public TypeMob TypeMob { get; }

    public event UnityAction<IMob> CameOut;
    public event UnityAction<IMob> Deaded;
}
