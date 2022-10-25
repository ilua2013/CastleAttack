using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    Transform TransformPoint { get; }
    void Init(Transform transformPoint);
}
