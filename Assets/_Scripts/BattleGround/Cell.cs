using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private List<UnitSpawner> _unitSpawners;
    
    private Transform _targetCell;

    public void Init(Transform targetPoint, Button button)
    {
        foreach (var spawners in _unitSpawners)
        {
            spawners.Init(targetPoint, button);
        }
    }
}
