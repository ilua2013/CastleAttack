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
        if (_cellIs != CellIs.Lower)
            return;

        Cell currentCell = this;
        int number = 0;

        //while (currentCell.Bot != null)
        //    currentCell = currentCell.Bot;

        while (currentCell != null)
        {
            currentCell.SetNumber(number);
            number++;
            currentCell = currentCell.Top;
        }
    }

    public void SetNumber(int value)
    {
        _number = value;
    }

    public void SetType(CellIs cellIs)
    {
        _cellIs = cellIs;
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

    public List<Cell> GetCellsDistanceAttack(DistanceAttack[] distanceAttacks)
    {
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < distanceAttacks.Length; i++)
        {
            switch (distanceAttacks[i].Side)
            {
                case CellNeighbor.Bot:
                    SetCellsInCells(cells , GetBotCells(distanceAttacks[i].Distance));
                    break;

                case CellNeighbor.Left:
                    SetCellsInCells(cells, GetLeftCells(distanceAttacks[i].Distance));
                    break;

                case CellNeighbor.Top:
                    SetCellsInCells(cells, GetTopCells(distanceAttacks[i].Distance));
                    break;

                case CellNeighbor.Right:
                    SetCellsInCells(cells, GetRightCells(distanceAttacks[i].Distance));
                    break;

                case CellNeighbor.TopLeft:
                    SetCellsInCells(cells, GetTopLeftCells(distanceAttacks[i].Distance));
                    break;

                case CellNeighbor.TopRight:
                    SetCellsInCells(cells, GetTopRightCells(distanceAttacks[i].Distance));
                    break;

                case CellNeighbor.BotLeft:
                    SetCellsInCells(cells, GetBotLeftCells(distanceAttacks[i].Distance));
                    break;

                case CellNeighbor.BotRight:
                    SetCellsInCells(cells, GetBotRightCells(distanceAttacks[i].Distance));
                    break;
            }
        }

        return cells;
    }

    public List<UnitEnemy> GetEnemyUnits(DistanceAttack[] distanceAttack)
    {
        List<UnitEnemy> enemys = new List<UnitEnemy>();

        for (int i = 0; i < distanceAttack.Length; i++)
        {
            switch (distanceAttack[i].Side)
            {
                case CellNeighbor.Bot:
                    CheckStateUnitEnemy(GetBotCells(distanceAttack[i].Distance), enemys);
                    break;

                case CellNeighbor.Left:
                    CheckStateUnitEnemy(GetLeftCells(distanceAttack[i].Distance), enemys);
                    break;

                case CellNeighbor.Top:
                    CheckStateUnitEnemy(GetTopCells(distanceAttack[i].Distance), enemys);
                    break;

                case CellNeighbor.Right:
                    CheckStateUnitEnemy(GetRightCells(distanceAttack[i].Distance), enemys);
                    break;

                case CellNeighbor.TopLeft:
                    CheckStateUnitEnemy(GetTopLeftCells(distanceAttack[i].Distance), enemys);
                    break;

                case CellNeighbor.TopRight:
                    CheckStateUnitEnemy(GetTopRightCells(distanceAttack[i].Distance), enemys);
                    break;

                case CellNeighbor.BotLeft:
                    CheckStateUnitEnemy(GetBotLeftCells(distanceAttack[i].Distance), enemys);
                    break;

                case CellNeighbor.BotRight:
                    CheckStateUnitEnemy(GetBotRightCells(distanceAttack[i].Distance), enemys);
                    break;
            }
        }

        return enemys;
    }

    public List<UnitFriend> GetFriendUnits(DistanceAttack[] distanceAttack)
    {
        List<UnitFriend> friends = new List<UnitFriend>();

        for (int i = 0; i < distanceAttack.Length; i++)
        {
            switch (distanceAttack[i].Side)
            {
                case CellNeighbor.Bot:
                    CheckStateUnitFriend(GetBotCells(distanceAttack[i].Distance), friends);
                    break;

                case CellNeighbor.Left:
                    CheckStateUnitFriend(GetLeftCells(distanceAttack[i].Distance), friends);
                    break;

                case CellNeighbor.Top:
                    CheckStateUnitFriend(GetTopCells(distanceAttack[i].Distance), friends);
                    break;

                case CellNeighbor.Right:
                    CheckStateUnitFriend(GetRightCells(distanceAttack[i].Distance), friends);
                    break;

                case CellNeighbor.TopLeft:
                    CheckStateUnitFriend(GetTopLeftCells(distanceAttack[i].Distance), friends);
                    break;

                case CellNeighbor.TopRight:
                    CheckStateUnitFriend(GetTopRightCells(distanceAttack[i].Distance), friends);
                    break;

                case CellNeighbor.BotLeft:
                    CheckStateUnitFriend(GetBotLeftCells(distanceAttack[i].Distance), friends);
                    break;

                case CellNeighbor.BotRight:
                    CheckStateUnitFriend(GetBotRightCells(distanceAttack[i].Distance), friends);
                    break;
            }
        }

        return friends;
    }

    private void SetCellsInCells(List<Cell> defolt, List<Cell> add)
    {
        foreach (var item in add)
            if (defolt.Contains(item) == false)
                defolt.Add(item);
    }

    private void CheckStateUnitEnemy(List<Cell> cells, List<UnitEnemy> enemys)
    {
        foreach (var item in cells)
            if (item.IsFree == false && item.CurrentUnit is UnitEnemy unitEnemy)
                enemys.Add(unitEnemy);
    }

    private void CheckStateUnitFriend(List<Cell> cells, List<UnitFriend> enemys)
    {
        foreach (var item in cells)
            if (item.IsFree == false && item.CurrentUnit is UnitFriend unitEnemy)
                enemys.Add(unitEnemy);
    }

    private List<Cell> GetTopCells(int count)
    {
        Cell cureentCell = _top;
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < count; i++)
        {
            if (cureentCell == null)
                break;

            cells.Add(cureentCell);
            cureentCell = cureentCell.Top;
        }

        return cells;
    }

    private List<Cell> GetTopLeftCells(int count)
    {
        Cell cureentCell = _topLeft;
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < count; i++)
        {
            if (cureentCell == null)
                break;

            cells.Add(cureentCell);
            cureentCell = cureentCell._topLeft;
        }

        return cells;
    }

    private List<Cell> GetTopRightCells(int count)
    {
        Cell cureentCell = _topRight;
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < count; i++)
        {
            if (cureentCell == null)
                break;

            cells.Add(cureentCell);
            cureentCell = cureentCell._topRight;
        }

        return cells;
    }

    private List<Cell> GetRightCells(int count)
    {
        Cell cureentCell = _right;
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < count; i++)
        {
            if (cureentCell == null)
                break;

            cells.Add(cureentCell);
            cureentCell = cureentCell._right;
        }

        return cells;
    }

    private List<Cell> GetBotRightCells(int count)
    {
        Cell cureentCell = _botRight;
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < count; i++)
        {
            if (cureentCell == null)
                break;

            cells.Add(cureentCell);
            cureentCell = cureentCell._botRight;
        }

        return cells;
    }

    private List<Cell> GetBotCells(int count)
    {
        Cell cureentCell = _bot;
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < count; i++)
        {
            if (cureentCell == null)
                break;

            cells.Add(cureentCell);
            cureentCell = cureentCell._bot;
        }

        return cells;
    }

    private List<Cell> GetBotLeftCells(int count)
    {
        Cell cureentCell = _botLeft;
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < count; i++)
        {
            if (cureentCell == null)
                break;

            cells.Add(cureentCell);
            cureentCell = cureentCell._botLeft;
        }

        return cells;
    }

    private List<Cell> GetLeftCells(int count)
    {
        Cell cureentCell = _left;
        List<Cell> cells = new List<Cell>();

        for (int i = 0; i < count; i++)
        {
            if (cureentCell == null)
                break;

            cells.Add(cureentCell);
            cureentCell = cureentCell._left;
        }

        return cells;
    }
}

public enum CellIs
{
    Higher, Lower, Default
}

public enum CellNeighbor
{
    Left, Top, Right, Bot, TopLeft, TopRight, BotLeft, BotRight
}
