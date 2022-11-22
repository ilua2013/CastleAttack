using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projection : MonoBehaviour
{
    public abstract void Init(Cell cell);
    public abstract void Show(Cell cell);
}
