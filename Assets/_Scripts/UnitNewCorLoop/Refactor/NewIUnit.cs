using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface NewIUnit 
{
    NewMover Mover { get; }
    NewFighter Fighter { get; }
    Card Card { get; }
    void Init(Card card, Cell cell);
}
