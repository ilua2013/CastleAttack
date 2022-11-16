using System.Collections.Generic;
using UnityEngine;

public class RadiusAttackView : MonoBehaviour
{
    [field: SerializeField] public UnitView UnitView { get; private set; }

    private List<Cell> _cells = new List<Cell>();

    private void Start()
    {
        _cells = UnitView.ViewCells();
        SelectionCells(_cells);
    }

    private void OnEnable()
    {
        UnitView.UnitMover().Moved += OnMoved;
    }

    private void OnDisable()
    {
        UnitView.UnitMover().Moved -= OnMoved;
        UnitDisable();
    }

    private void OnMoved()
    {
        UnSelectionCells(_cells);
        _cells.Clear();
        _cells = UnitView.ViewCells();
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
                if (UnitView.Unit == Unit.Enemy)
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
                if (UnitView.Unit == Unit.Enemy)
                    highlighting.UnSelectEnemy();
                else
                    highlighting.UnSelect();
            }
        }
    }
}
