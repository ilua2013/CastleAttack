using System.Collections.Generic;
using UnityEngine;

public class RadiusAttackView : MonoBehaviour
{
    [SerializeField] private UnitView _unitView;

    private List<Cell> _cells = new List<Cell>();

    private void Start()
    {
        _cells = _unitView.ViewCells();
        SelectionCells(_cells);
    }

    private void OnEnable()
    {
        _unitView.UnitMover().Moved += OnMoved;       
    }

    private void OnDisable()
    {
        _unitView.UnitMover().Moved -= OnMoved;
        _cells.Clear();
    }

    private void OnMoved()
    {
        UnSelectionCells(_cells);
        _cells.Clear();
        _cells = _unitView.ViewCells();
        SelectionCells(_cells);
    }

    private void SelectionCells(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
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
                highlighting.UnSelect();
            }
        }
    }
}
