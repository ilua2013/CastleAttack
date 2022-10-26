using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IUnit
{
    Transform TransformPoint { get; }
    Card Card { get; }
    void Init(Card card, Transform transformPoint, Button button);
    void ReurnToHand();
}
