using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Mover
{
    [SerializeField] private Cell _startCell;
    [SerializeField] private bool _canMove = true;
    [Header("Rotate Params")]
    [SerializeField] private float _speedRotateToWizzard;

    private IUnit _unit;
    private Cell _currentCell;
    private Transform transform;
    private float _timeMove = 0.5f;
    private float _speedAnimation = 6f;
    private bool _init;

    public bool IsSkipped { get; private set; }
    public Cell CurrentCell => _currentCell;
    public IUnit Unit => _unit;

    public event Action ReachedHigherCell;
    public event Action ReachedTutorialHigherCell;
    public event Action Moved;
    public event Action<Cell> CellChanged;
    public event Action Rooted;
    public event Action UnRooted;
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

    public void SetCurrentCell(Cell cell, bool isInit = true)
    {
        if (_currentCell != null)
            _currentCell.SetFree();

        _currentCell = cell;
        _currentCell.StateUnitOnCell(_unit, isInit);
    }

    public void SkipStep()
    {
        IsSkipped = true;
    }

    public void Root()
    {
        Rooted?.Invoke();
    }

    public void UnRoot()
    {
        UnRooted?.Invoke();
        IsSkipped = false;
    }

    public bool CanMove(Cell cell)
    {
        if (IsSkipped)
        {
            IsSkipped = false;
            return false;
        }
        
        if (_canMove == false || cell == null || cell.IsFree == false)
            return false;
        else
            return true;
    }

    public void Move(Cell cell, Action onEnd = null)
    {
        _currentCell.SetFree();

        _unit.StartMove(cell, () =>
        {
            SetCurrentCell(cell,false);
            
            onEnd?.Invoke();
        });

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

    public IEnumerator MoveTo(Cell cell, Action onEnd = null)
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

        onEnd?.Invoke();
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

    public IEnumerator RotateTo(Vector3 targetEuler, Action onRotated = null, float time = 0.5f)
    {
        Vector3 target = targetEuler - transform.position;
        Vector3 startForward = transform.forward;
        Vector3 forward = transform.forward;
        float ellapsedTime = 0;
        float percent = 0;
        float timeMax = 0;
        target.y = 0;

        while (transform.eulerAngles != targetEuler)
        {
            ellapsedTime = ellapsedTime > time ? time : ellapsedTime + Time.deltaTime;
            timeMax += Time.deltaTime;
            percent = ellapsedTime / time;

            transform.rotation = Quaternion.RotateTowards(Quaternion.Euler(transform.eulerAngles), Quaternion.Euler(targetEuler), 1100 * Time.deltaTime);

            //transform.forward = startForward.normalized * (1 - percent) + (target.normalized * percent);
            //forward = Vector3.MoveTowards(forward, target.normalized, 15 * Time.deltaTime);
            //transform.forward = forward;
            //transform.forward = Vector3.RotateTowards(transform.forward, target, 15 * Time.deltaTime, 15 * Time.deltaTime);
            yield return null;
        }
        onRotated?.Invoke();
    }
}
