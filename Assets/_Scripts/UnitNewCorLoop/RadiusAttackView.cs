using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusAttackView : MonoBehaviour
{
    [SerializeField] private UnitFriend _unitFriend;

    private void Start()
    {
        foreach (var cell in _unitFriend.Mover.CurrentCell.GetForwardsCell(_unitFriend.Fighter.DistanceAttack))
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
                highlighting.Select();
        }
    }

    private void OnEnable()
    {
        _unitFriend.Mover.Moved += OnMoved;
    }

    private void OnDisable()
    {
        _unitFriend.Mover.Moved -= OnMoved;
    }

    private void OnMoved()
    {
        List<Cell> forwardCells =_unitFriend.Mover.CurrentCell.GetForwardsCell(1);
        List<Cell> backCells =_unitFriend.Mover.CurrentCell.GetBottomCell(1);

        foreach (Cell cell in forwardCells)
        {
            if (cell.TryGetComponent(out HighlightingCell highlightingCell))
                highlightingCell.Select();
        }

        foreach (Cell cell in backCells)
        {
            if (cell.TryGetComponent(out HighlightingCell highlightingCell))
                highlightingCell.UnSelect();
        }
    }
}
