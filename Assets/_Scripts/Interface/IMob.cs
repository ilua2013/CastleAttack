using TypesMobs;
using UnityEngine;
using UnityEngine.Events;

public interface IMob
{
    public void TakeDamage(int damage);

    public Vector3 TransformPosition { get; }
    public TypeMob TypeMob { get; }

    public event UnityAction<IMob> CameOut;
    public event UnityAction<IMob> Deaded;

}
