using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Mover
{
    [SerializeField] private Cell _startCell;
    [SerializeField] private float _timeMove;
    [SerializeField] private bool _canMove = true;

    private IUnit _unit;
    private Cell _currentCell;
    private Transform transform;

    public Cell CurrentCell => _currentCell;
    public IUnit Unit => _unit;

    public event Action ReachedHigherCell;
    public event Action Moved;
    public event Action<Cell> CellChanged;

    public void Init(IUnit unit, Transform unitTransform, Cell cell = null)
    {
        _unit = unit;
        transform = unitTransform;

        if (cell == null && _startCell != null)
            SetCurrentCell(_startCell);
        else
            SetCurrentCell(cell);
    }

    public void SetStartCell(Cell cell)
    {
        _startCell = cell;
    }

    public void SetCurrentCell(Cell cell)
    {
        if (_currentCell != null)
            _currentCell.SetFree();

        _currentCell = cell;
        _currentCell.StateUnitOnCell(_unit);
    }

    public bool CanMove(Cell cell)
    {
        if (_canMove == false || cell == null || cell.IsFree == false)
            return false;
        else
            return true;
    }

    public void Move(Cell cell)
    {
        _currentCell.SetFree();
        SetCurrentCell(cell);

        Moved?.Invoke();
        CellChanged?.Invoke(cell);
    }

    public void Die()
    {
        if (_currentCell == null)
            return;

        _currentCell.SetFree();
        //_currentCell = null;
    }

    public void SetTransformOnCell(Cell cell)
    {
        SetCurrentCell(cell);
        transform.position = cell.transform.position;
    }

    public IEnumerator MoveTo(Cell cell)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = cell.transform.position - transform.position;
        float time = 0;

        if (cell.CellIs == CellIs.Higher || cell.CellIs == CellIs.Boss)
        {
            if (cell.Top == null)
                ReachedHigherCell?.Invoke();
        }

        while (time < _timeMove)
        {
            time = time > _timeMove ? _timeMove : time + Time.deltaTime;

            transform.position = startPos + (targetPos * (time / _timeMove));
            yield return null;
        }
    }
}
