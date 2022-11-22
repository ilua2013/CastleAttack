using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRadiusAttack
{
    Mover Mover { get; }
    public DistanceAttack[] DistanceAttack { get; }

    public List<Cell> RadiusView();

    public event Action Inited;
}
