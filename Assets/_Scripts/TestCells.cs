using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCells : MonoBehaviour
{
    public Cell _cell;
    public Set Set;

    private void OnValidate()
    {
        _cell = GetComponentInParent<Cell>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Cell cell) && cell != _cell)
        {
            _cell.SetCell(cell, Set);
        }
    }
}
public enum Set
{
    Left, Top, Right, Bot
}
