using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Mover
{
    [SerializeField] private Cell _startCell;
    [SerializeField] private bool _canMove = true;

    private IUnit _unit;
    private Cell _currentCell;
    private Transform transform;
    private float _timeMove = 0.6f;
    private float _speedAnimation = 6f;
    private bool _init;

    public Cell CurrentCell => _currentCell;
    public IUnit Unit => _unit;

    public event Action ReachedHigherCell;
    public event Action ReachedTutorialHigherCell;
    public event Action Moved;
    public event Action<Cell> CellChanged;
    //public event Action Died;

    public void Init(IUnit unit, Transform unitTransform, Cell cell = null)
    {
        _unit = unit;
        transform = unitTransform;

        if (cell == null && _startCell != null)
            SetCurrentCell(_startCell);
        else
            SetCurrentCell(cell);

        if (_init == false)
            _unit.AnimationSizeUp();

        _init = true;
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
        //Died?.Invoke();
        if (_currentCell == null)
            return;

        _currentCell.SetFree();
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

        if (cell.CellIs == CellIs.Higher)
        {
            ReachedTutorialHigherCell?.Invoke();
        }

        if (cell.CellIs == CellIs.Higher || cell.CellIs == CellIs.Boss)
        {
            if (cell.Top == null)
            {
                ReachedHigherCell?.Invoke();
            }
        }       

        float times = 0;
        while (time < _timeMove)
        {
            time = time > _timeMove ? _timeMove : time + Time.deltaTime;
            times += Time.deltaTime;
            transform.position = startPos + (targetPos * (time / _timeMove));
            yield return null;
        }
    }
    
    public IEnumerator AnimationSizeUp()
    {
        Vector3 startSize = transform.localScale;
        transform.localScale = Vector3.zero;

        while (Vector3.Distance(startSize, transform.localScale) > 0.001f)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, startSize, _speedAnimation * Time.deltaTime);
            yield return null;
        }
    }
}
