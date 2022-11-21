using System;
using UnityEngine;

public interface IUnit 
{
    Mover Mover { get; }
    Fighter Fighter { get; }
    UnitCard Card { get; }
    bool Initialized { get; }
    public event Action Inited;
    void Init(UnitCard card, Cell cell);
    void DoStep();
    void RotateTo(Transform transform);
}
