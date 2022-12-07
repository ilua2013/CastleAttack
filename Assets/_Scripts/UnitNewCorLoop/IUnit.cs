using System;
using UnityEngine;
using System.Collections;

public interface IUnit
{
    Mover Mover { get; }
    Fighter Fighter { get; }
    UnitCard Card { get; }
    public event Action FinishedStep;
    public event Action StepChanged;
    public event Action EndedSteps;
    int MaxStep { get; }
    int CurrentStep { get; }
    bool Initialized { get; }
    public event Action Inited;
    void Init(UnitCard card, Cell cell);
    void DoStep();
    void RotateTo(Transform transform, Action onRotated = null);
    void AnimationSizeUp();
}
