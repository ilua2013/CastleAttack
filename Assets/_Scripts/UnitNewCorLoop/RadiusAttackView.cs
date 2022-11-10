using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiusAttackView : MonoBehaviour
{
    [SerializeField] private UnitFriend _unitFriend;

    private List<HighlightingCell> _highlightingTopCell = new List<HighlightingCell>();
    private List<HighlightingCell> _highlightingBottomCell = new List<HighlightingCell>();
    private List<HighlightingCell> _highlightingLeftCell = new List<HighlightingCell>();
    private List<HighlightingCell> _highlightingRightCell = new List<HighlightingCell>();

    private void OnValidate()
    {
        _unitFriend = GetComponent<UnitFriend>();
    }

    private void Start()
    {
        foreach (var cell in _unitFriend.Mover.CurrentCell.GetTopCell(_unitFriend.TopDistance))
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                _highlightingTopCell.Add(highlighting);
                highlighting.Select();
            }

        }

        foreach (var cell in _unitFriend.Mover.CurrentCell.GetBottomCell(_unitFriend.BottomDistance))
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                _highlightingBottomCell.Add(highlighting);
                highlighting.Select();
            }
        }

        foreach (var cell in _unitFriend.Mover.CurrentCell.GetRightCell(_unitFriend.RightDistance))
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                _highlightingRightCell.Add(highlighting);
                highlighting.Select();
            }
        }

        foreach (var cell in _unitFriend.Mover.CurrentCell.GetLeftCell(_unitFriend.LeftDistance))
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                _highlightingLeftCell.Add(highlighting);
                highlighting.Select();
            }
        }
    }

    private void OnEnable()
    {
        _unitFriend.Mover.Moved += OnMoved;
    }

    private void OnDisable()
    {
        _unitFriend.Mover.Moved -= OnMoved;

        foreach (var highlightingCell in _highlightingTopCell)
        {
            highlightingCell.UnSelect();
        }
        foreach (var highlightingCell in _highlightingBottomCell)
        {
            highlightingCell.UnSelect();
        }
        foreach (var highlightingCell in _highlightingLeftCell)
        {
            highlightingCell.UnSelect();
        }
        foreach (var highlightingCell in _highlightingRightCell)
        {
            highlightingCell.UnSelect();
        }
        _highlightingTopCell.Clear();
        _highlightingBottomCell.Clear();
        _highlightingRightCell.Clear();
        _highlightingLeftCell.Clear();

    }

    private void OnMoved()
    {

        foreach (var highlightingCell in _highlightingTopCell)
        {
            highlightingCell.UnSelect();
        }
        foreach (var highlightingCell in _highlightingBottomCell)
        {
            highlightingCell.UnSelect();
        }
        foreach (var highlightingCell in _highlightingLeftCell)
        {
            highlightingCell.UnSelect();
        }
        foreach (var highlightingCell in _highlightingRightCell)
        {
            highlightingCell.UnSelect();
        }
        _highlightingTopCell.Clear();
        _highlightingBottomCell.Clear();
        _highlightingRightCell.Clear();
        _highlightingLeftCell.Clear();
        foreach (var cell in _unitFriend.Mover.CurrentCell.GetTopCell(_unitFriend.TopDistance))
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                _highlightingTopCell.Add(highlighting);
                highlighting.Select();
            }
        }
        foreach (var cell in _unitFriend.Mover.CurrentCell.GetBottomCell(_unitFriend.BottomDistance))
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                _highlightingBottomCell.Add(highlighting);
                highlighting.Select();
            }
        }
        foreach (var cell in _unitFriend.Mover.CurrentCell.GetLeftCell(_unitFriend.LeftDistance))
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                _highlightingLeftCell.Add(highlighting);
                highlighting.Select();
            }
        }
        foreach (var cell in _unitFriend.Mover.CurrentCell.GetRightCell(_unitFriend.RightDistance))
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                _highlightingRightCell.Add(highlighting);
                highlighting.Select();
            }
        }
    }
}
