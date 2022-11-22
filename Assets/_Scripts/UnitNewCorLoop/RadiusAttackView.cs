using System.Collections.Generic;
using UnityEngine;

public class RadiusAttackView : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _unit;
    [SerializeField] private Unit _unitType;

    private IRadiusAttack _radiusAttack;

    private List<Cell> _cells = new List<Cell>();

    public List<Cell> ViewCells()
    {
        return _cells = _radiusAttack.RadiusView();
    }

    public void ShowRadius(Cell cell, DistanceAttack[] attacks)
    {
        UnSelectionCells(_cells);
        _cells.Clear();
        _cells = cell.GetCellsDistanceAttack(attacks);
        _cells.Add(cell);
        SelectionCells(_cells);
    }

    public void ShowRadius(Cell cell)
    {
        UnSelectionCells(_cells);
        _cells.Clear();
        _cells = cell.GetCellsDistanceAttack(_radiusAttack.DistanceAttack);
        _cells.Add(cell);
        SelectionCells(_cells);
    }

    public void ShowRadius()
    {
        UnSelectionCells(_cells);
        _cells.Clear();
        _cells = ViewCells();
        SelectionCells(_cells);
    }

    private void OnEnable()
    {
        _radiusAttack = _unit.GetComponent<IRadiusAttack>();
        _radiusAttack.Inited += InitedCells;

        if (_radiusAttack.Mover != null)
            _radiusAttack.Mover.Moved += OnMoved;
    }

    private void OnDisable()
    {
        _radiusAttack.Inited -= InitedCells;

        if (_radiusAttack.Mover != null)
            _radiusAttack.Mover.Moved -= OnMoved;

        UnitDisable();
    }

    private void OnMoved()
    {
        ShowRadius();
    }

    private void InitedCells()
    {
        ShowRadius();
    }

    private void UnitDisable()
    {
        UnSelectionCells(_cells);
        _cells.Clear();
    }

    private void SelectionCells(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                if (_unitType == Unit.Enemy)
                    highlighting.SelectEnemy();
                else
                    highlighting.Select();
            }
        }
    }

    private void UnSelectionCells(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell != null)
            {
                if (cell.TryGetComponent(out HighlightingCell highlighting))
                {
                    if (_unitType == Unit.Enemy)
                        highlighting.UnSelectEnemy();
                    else
                        highlighting.UnSelect();
                }
            }
        }
    }
}

public enum Unit
{
    Friend, Enemy
}
