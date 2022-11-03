using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NewMover
{
    [SerializeField] private Cell _startCell;
    [SerializeField] private float _speed;
    [SerializeField] private bool _canMove = true;

    private NewIUnit _iUnit;
    private Cell _currentCell;
    private Transform transform;

    public Cell CurrentCell => _currentCell;

    public event Action ReachedHigherCell;
    public event Action<Cell> CellChanged;

    public void Init(NewIUnit unit, Transform unitTransform, Cell cell = null)
    {
        _iUnit = unit;
        transform = unitTransform;

        SetCurrentCell(cell);

        if (cell == null && _startCell != null)
            SetCurrentCell(_startCell);
    }

    public void SetCurrentCell(Cell cell)
    {
        _currentCell = cell;
        _currentCell.StateUnitOnCell(_iUnit);
    }

    public bool CanMove(Cell cell)
    {
        if (_canMove == false || cell.IsFree == false)
            return false;
        else
            return true;
    }

    public void Move(Cell cell)
    {
        _currentCell.SetFree();
        SetCurrentCell(cell);

        CellChanged?.Invoke(cell);
    }

    public void Die()
    {
        _currentCell.SetFree();
        _currentCell = null;
    }

    public IEnumerator MoveTo(Cell cell)
    {
        while (Vector3.Distance(transform.position, cell.transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, cell.transform.position, _speed * Time.deltaTime);
            yield return null;
        }

        if (cell.CellIs == CellIs.Higher && cell.Top == null)
            ReachedHigherCell?.Invoke();
    }
}
