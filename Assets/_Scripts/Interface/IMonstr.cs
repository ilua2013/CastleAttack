using UnityEngine;
using UnityEngine.Events;

public interface IMonstr
{
    public void TakeDamage(int damage);

    public Vector3 TransformPosition { get; }

    public event UnityAction<IMonstr> CameOut;
    public event UnityAction<IMonstr> Deaded;
}