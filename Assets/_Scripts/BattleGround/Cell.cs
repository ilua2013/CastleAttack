using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Cell : MonoBehaviour
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Button _startFightButton;
    
    private List<UnitSpawner> _unitSpawners = new List<UnitSpawner>();

    public Transform TargetPoint => _targetPoint;
    public Button ButtonStartFight => _startFightButton;

    private void OnValidate()
    {
        _startFightButton = FindObjectOfType<CellSpawner>().ButtonStartFight;
        _unitSpawners.Clear();

        foreach (var spawner in GetComponents<UnitSpawner>())
        {
            _unitSpawners.Add(spawner);
        }
    }
}
