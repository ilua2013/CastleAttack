using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PhaseType
{
    SelectionCard,
    Battle,
    CardPlacement
}

[Serializable]
public class Phase
{
    [SerializeField] private PhaseType _phaseType;
    [SerializeField] private bool _isActive;
    [SerializeField] private float _delayTime;

    public PhaseType PhaseType => _phaseType;
    public bool IsActive => _isActive;
    public float Delay => _delayTime;
}
