using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeckCounter : MonoBehaviour
{
    public bool CanTakeCard => true;

    public event Action<int> Decreased;
}
