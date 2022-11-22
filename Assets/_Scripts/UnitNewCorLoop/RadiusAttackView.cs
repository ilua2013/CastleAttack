using System.Collections.Generic;
using UnityEngine;

public class RadiusAttackView : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _unit;
    [SerializeField] private Unit _unitType;

    private IRadiusAttack _radiusAttack;

    private List<Cell> _cells = new List<Cell>();
    private bool _isInited = false;

    public Mover UnitMover()
    {
        return _radiusAttack.Mover;
    }

    public List<Cell> ViewCells()
    {
        return _cells = _radiusAttack.RadiusView();
    }   

    private void OnEnable()
    {
        _radiusAttack = _unit.GetComponent<IRadiusAttack>();
        _radiusAttack.Inited += InitedCells;
        UnitMover().Moved += OnMoved;
    }

    private void OnDisable()
    {
        _radiusAttack.Inited -= InitedCells;
        UnitMover().Moved -= OnMoved;
        UnitDisable();
    }

    private void OnMoved()
    {
        UnSelectionCells(_cells);
        _cells.Clear();
        _cells = ViewCells();
        SelectionCells(_cells);
    }

    private void InitedCells()
    {
        if(_isInited == false)
        {
            _cells = ViewCells();
            SelectionCells(_cells);
            _isInited = true;
        }
       
    }

    private void UnitDisable()
    {
        UnSelectionCells(_cells);
        _cells.Clear();
    }

    private void SelectionCells(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell.TryGetComponent(out HighlightingCell highlighting))
            {
                if (_unitType == Unit.Enemy)
                    highlighting.SelectEnemy();
                else
                    highlighting.Select();
            }
        }
    }

    private void UnSelectionCells(List<Cell> cells)
    {
        foreach (var cell in cells)
        {
            if (cell != null)
            {
                if (cell.TryGetComponent(out HighlightingCell highlighting))
                {
                    if (_unitType == Unit.Enemy)
                        highlighting.UnSelectEnemy();
                    else
                        highlighting.UnSelect();
                }
            }
        }
    }
}

public enum Unit
{
    Friend, Enemy
}
