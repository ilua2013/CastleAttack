using System.Collections.Generic;
using UnityEngine;

public class RadiusAttackView : MonoBehaviour
{
    [SerializeField] private UnitFriend _unitFriend;
    [SerializeField] private UnitEnemy _unitEnemy;
    [SerializeField] private Unit _unit;

    private List<Cell> _cells = new List<Cell>();

    public Mover UnitMover()
    {
        if (_unit == Unit.Friend)
            return _unitFriend.Mover;
        else
            return _unitEnemy.Mover;
    }

    public List<Cell> ViewCells()
    {
        if (_unit == Unit.Friend)
            return _cells = _unitFriend.RadiusView();
        else
            return _cells = _unitEnemy.RadiusView();
    }

    private void Start()
    {
        _cells = ViewCells();
        SelectionCells(_cells);
    }

    private void OnEnable()
    {
        UnitMover().Moved += OnMoved;
    }

    private void OnDisable()
    {
        UnitMover().Moved -= OnMoved;
        UnitDisable();
    }

    private void OnMoved()
    {
        UnSelectionCells(_cells);
        _cells.Clear();
        _cells = ViewCells();
        SelectionCells(_cells);
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
                if (_unit == Unit.Enemy)
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
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                if (_unit == Unit.Enemy)
                    highlighting.UnSelectEnemy();
                else
                    highlighting.UnSelect();
            }
        }
    }
}
public enum Unit
{
    Friend, Enemy
}
