using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellIs _cellIs;
    [Header("CellNeighbor")]
    [SerializeField] private Cell _top;
    [SerializeField] private Cell _bot;
    [SerializeField] private Cell _left;
    [SerializeField] private Cell _right;

    public Cell Top => _top;
    public Cell Bot => _bot;
    public Cell Left => _left;
    public Cell Right => _right;
    public CellIs CellIs => _cellIs;

    private List<UnitSpawner> _unitSpawners = new List<UnitSpawner>();

    private void OnValidate()
    {
        _unitSpawners.Clear();

        foreach (var spawner in GetComponents<UnitSpawner>())
        {
            _unitSpawners.Add(spawner);
        }
    }

    public void SetCell(Cell cell, Set set)
    {
        switch (set)
        {
            case Set.Top:
                _top = cell;
                break;
            case Set.Bot:
                _bot = cell;
                break;
            case Set.Right:
                _right = cell;
                break;
            case Set.Left:
                _left = cell;
                break;
        }
    }
}

public enum CellIs
{
    Higher, Lower, Default
}
