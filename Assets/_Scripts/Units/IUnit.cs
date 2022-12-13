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
    public event Action<bool> UnitSteped;
    public event Action StartedWalking;
    int MaxStep { get; }
    int CurrentStep { get; }
    bool Initialized { get; }
   
    public event Action Inited;
    void Init(UnitCard card, Cell cell);
    void DoStep(IUnit enemy = null);
    void RotateTo(Transform transform, Action onRotated = null, Action onEnd = null);
    void LocalRotateTo(Transform transform, Action onRotated = null, Action onEnd = null);
    void AnimationSizeUp();
    void StartMove(Cell cell, Action onEnd = null);
    IEnumerator FinishStep(Action action, float time);
    Arrow SpawnArrow(Arrow arrow, Vector3 position);
}
