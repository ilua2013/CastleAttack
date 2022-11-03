using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NewUnitEnemy : NewIUnit
{
    [SerializeField] private int _maxStep = 3;
    [field: SerializeField] public NewMover Mover { get; private set; }
    [field: SerializeField] public NewFighter Fighter { get; private set; }

    public Card Card { get; private set; }
    private int _currentStep;

    public int CurrentStep => _currentStep;

    public event Action Returned;
    public event Action Attacked;
    public event Action Moved;
    public event Action Inited;
    public event Action EndedSteps;

    public void Init(Card card, Cell cell)
    {
        throw new System.NotImplementedException();
    }
}
