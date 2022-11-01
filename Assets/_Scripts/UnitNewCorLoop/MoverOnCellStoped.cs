using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverOnCellStoped : MoverOnCell
{
    
    public event Action Damadge;


    public override void Move(TeamUnit teamUnit)
    {       
        _currentCell.StateUnitOnCell(GetComponent<UnitStep>());
    }
}
