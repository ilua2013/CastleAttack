using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitFriend))]
public class RadiusAttackView : MonoBehaviour
{

    [SerializeField] private UnitFriend _unitFriend;
    private List<Cell> _cells = new List<Cell>();

    private void Start()
    {
        _cells = _unitFriend.RadiusView();
        SelectionCells(_cells);

    }

    private void OnEnable()
    {       
        _unitFriend.Mover.Moved += OnMoved;
    }

    private void OnDisable()
    {
        _unitFriend.Mover.Moved -= OnMoved;       
        _cells.Clear();
    }

    private void OnMoved()
    {
        UnSelectionCells(_cells);
        _cells.Clear();
        _cells = _unitFriend.RadiusView();
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
