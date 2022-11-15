using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private UnitFriend _unitFriend;
    [SerializeField] private UnitEnemy _unitEnemy;
    [SerializeField] private Unit _unit;

    private List<Cell> _cells = new List<Cell>();

    public Mover UnitMover()
    {
        if (_unit == Unit.Friend)
            return _unitFriend.Mover;

        else
            return _unitEnemy.Mover;
    }


    public List<Cell> ViewCells()
    {
        if (_unit == Unit.Friend)       
            return _cells = _unitFriend.RadiusView();
       
        else       
            return _cells = _unitEnemy.RadiusView();
       
    }
}
public enum Unit
{
    Friend, Enemy
}
