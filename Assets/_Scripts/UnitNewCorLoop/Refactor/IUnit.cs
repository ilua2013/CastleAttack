using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit 
{
    Mover Mover { get; }
    Fighter Fighter { get; }
    Card Card { get; }
    void Init(Card card, Cell cell);
}
