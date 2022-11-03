using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoverOnCell : MonoBehaviour
{
    [SerializeField] private Cell _startCell;
    [SerializeField] private float _speed;
    [SerializeField] private bool _canMove = true;

    protected Cell _currentCell;

    public Cell CurrentCell => _currentCell;

    public event Action ReachedLastCell;

    private void Start()
    {
        if (_startCell != null)
            SetCurrentCell(_startCell);
    }

    public void SetCurrentCell(Cell cell)
    {
        _currentCell = cell;
        //_currentCell.StateUnitOnCell(GetComponent<UnitStep>());
    }

    public void Move(TeamUnit teamUnit)
    {
        if (_canMove == false)
            return;

        Cell target;

        if (teamUnit == TeamUnit.Friend)
            target = _currentCell.Top;
        else
            target = _currentCell.Bot;

        if (teamUnit == TeamUnit.Friend && _currentCell.Top.CellIs == CellIs.Higher && _currentCell.Top.Top == null)
            ReachedLastCell?.Invoke();
        else
            StartCoroutine(MoveTo(target.transform));

        _currentCell.SetFree();
        _currentCell = target;
        //_currentCell.StateUnitOnCell(GetComponent<UnitStep>());
    }

    public bool CanMove(TeamUnit teamUnit)
    {
        if (_canMove == false) // так нужно для типа build
            return true;

        if (teamUnit == TeamUnit.Friend && _currentCell.CellIs == CellIs.Higher)
        {
            if (_currentCell.Top == null)
                ReachedLastCell?.Invoke();

            return false;
        }
        else if (teamUnit == TeamUnit.Enemy && _currentCell.CellIs == CellIs.Lower)
            return false;

        if (teamUnit == TeamUnit.Friend)
        {
            //if (_currentCell.Top.IsFree == false && _currentCell.Top.CurrentUnit.TeamUnit == teamUnit && _currentCell.Top.CurrentUnit.CurrentStep == 0)
            //    return true;

            return _currentCell.Top.IsFree;
        }
        else
        {
            //if (_currentCell.Bot.IsFree == false && _currentCell.Bot.CurrentUnit.TeamUnit == teamUnit && _currentCell.Bot.CurrentUnit.CurrentStep == 0)
            //    return true;

            return _currentCell.Bot.IsFree;
        }
    }

    public void Die()
    {
        _currentCell.SetFree();
    }

    private IEnumerator MoveTo(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
            yield return null;
        }
    }
}
