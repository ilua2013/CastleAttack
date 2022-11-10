using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private CellIs _cellIs;
    [SerializeField] private int _number;
    [Header("CellNeighbor")]
    [SerializeField] private Cell _top;
    [SerializeField] private Cell _topLeft;
    [SerializeField] private Cell _topRight;
    [SerializeField] private Cell _bot;
    [SerializeField] private Cell _botLeft;
    [SerializeField] private Cell _botRight;
    [SerializeField] private Cell _left;
    [SerializeField] private Cell _right;

    private IUnit _currentUnit;

    public Cell Top => _top;
    public Cell TopRight => _topRight;
    public Cell TopLeft => _topLeft;
    public Cell Bot => _bot;
    public Cell BotRight => _botRight;
    public Cell BotLeft => _botLeft;
    public Cell Left => _left;
    public Cell Right => _right;
    public CellIs CellIs => _cellIs;
    public IUnit CurrentUnit => _currentUnit;
    public int Number => _number;
    public bool IsFree => _currentUnit == null;

    private void OnValidate()
    {
        //List<Cell> cells = new List<Cell>();
        //Cell currentCell = this;
        //int number = 0;

        //while (currentCell.Top != null)
        //    currentCell = currentCell.Top;

        //while (currentCell != null)
        //{
        //    cells.Add(currentCell);
        //    currentCell.SetNumber(number);
        //    number++;
        //    currentCell = currentCell.Bot;
        //}
    }

    public void SetNumber(int value)
    {
        _number = value;
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
            case CellNeighbor.TopLeft:
                _topLeft = cell;
                break;
            case CellNeighbor.TopRight:
                _topRight = cell;
                break;
            case CellNeighbor.BotLeft:
                _botLeft = cell;
                break;
            case CellNeighbor.BotRight:
                _botRight = cell;
                break;
        }
    }

    public void StateUnitOnCell(IUnit IUnit)
    {
        _currentUnit = IUnit;
    }

    public void SetFree()
    {
        _currentUnit = null;
    }

    public List<IUnit> GetUnits(List<Cell> cells)
    {
        List<IUnit> units = new List<IUnit>();

        foreach (var item in cells)
            if (item.IsFree == false)
                units.Add(item.CurrentUnit);

        return units;
    }

    public List<Cell> GetForwardsCell(int countForward)
    {
        List<Cell> cells = new List<Cell>();
        Cell currentCell = this;

        for (int i = 0; i < countForward; i++)
        {
            if (currentCell != null && currentCell.Top != null)
            {
                cells.Add(currentCell.Top);
                currentCell = currentCell.Top != null ? currentCell.Top : null;
            }
        }
        return cells;
    }

    public List<Cell> GetBottomCell(int countForward)
    {
        List<Cell> cells = new List<Cell>();
        Cell currentCell = this;

        for (int i = 0; i < countForward; i++)
        {
            if (currentCell != null)
            {
                cells.Add(currentCell.Bot);
                currentCell = currentCell.Bot != null ? currentCell.Bot : null;
            }
        }
        return cells;
    }

    public List<Cell> GetVerticalCells()
    {
        List<Cell> cells = new List<Cell>();
        Cell currentCell = this;

        while(currentCell.Top != null)
            currentCell = currentCell.Top;

        while(currentCell != null)
        {
            cells.Add(currentCell);
            currentCell = currentCell.Bot;
        }

        return cells;
    }
}

public enum CellIs
{
    Higher, Lower, Default
}
