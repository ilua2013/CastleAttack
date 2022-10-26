using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUnit
{
    Transform TransformPoint { get; }
    void Init(Transform transformPoint, Button button);
}
