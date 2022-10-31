using UnityEngine;
using UnityEngine.Events;

public interface IMonstr
{
    public void RecoveryHealth(int amount);

    public Vector3 TransformPosition { get; }

    public event UnityAction<IMonstr, IUnit> CameOut;
    public event UnityAction<IMonstr, IUnit> Deaded;
    public event UnityAction Returned;
}
