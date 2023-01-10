using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitFriend))]
public class RadiusAttackViewFriend : MonoBehaviour
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
        _unitFriend.Mover.StartedMove += OnMoved;
    }

    private void OnDisable()
    {
        _unitFriend.Mover.StartedMove -= OnMoved;       
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
        //if (_unitFriend.DistanceAttack[0].Distance < 2)
        //    return;

        foreach (var cell in cells)
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {               
                highlighting.SelectFriend();               
            }
        }
    }
    private void UnSelectionCells(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {              
                highlighting.UnSelectFriend();
            }
        }
    }   
}
