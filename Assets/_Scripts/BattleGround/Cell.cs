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

    private UnitStep _currentUnit;

    public Cell Top => _top;
    public Cell Bot => _bot;
    public Cell Left => _left;
    public Cell Right => _right;
    public CellIs CellIs => _cellIs;
    public UnitStep CurrentUnit => _currentUnit;
    public bool IsFree => _currentUnit == null;

    private List<UnitSpawner> _unitSpawners = new List<UnitSpawner>();

    private void OnValidate()
    {
        _unitSpawners.Clear();

        foreach (var spawner in GetComponents<UnitSpawner>())
        {
            _unitSpawners.Add(spawner);
        }
    }

    public void SetCell(Cell cell, CellNeighbor set)
    {
        switch (set)
        {
            case CellNeighbor.Top:
                _top = cell;
                break;
            case CellNeighbor.Bot:
                _bot = cell;
                break;
            case CellNeighbor.Right:
                _right = cell;
                break;
            case CellNeighbor.Left:
                _left = cell;
                break;
        }
    }

    public void StateUnitOnCell(UnitStep unit)
    {
        _currentUnit = unit;
    }

    public void SetFree()
    {
        _currentUnit = null;
    }

    public List<Cell> GetForwardsCell(int countForward)
    {
        List<Cell> cells = new List<Cell>();
        Cell currentCell = _top;

        for (int i = 0; i < countForward; i++)
        {
            cells.Add(currentCell.Top);
            currentCell = currentCell.Top;
        }
        print(cells.Count + " Count Cell Forward " + countForward);
        return cells;
    }

    public List<Cell> GetBottomCell(int countForward)
    {
        List<Cell> cells = new List<Cell>();
        Cell currentCell = _bot;

        for (int i = 0; i < countForward; i++)
        {
            if (currentCell != null)
                cells.Add(currentCell.Bot);

            currentCell = currentCell.Bot != null ? currentCell.Bot : null;
        }
        print(cells.Count + " Count Cell Bot " + countForward);
        return cells;
    }
}

public enum CellIs
{
    Higher, Lower, Default
}
