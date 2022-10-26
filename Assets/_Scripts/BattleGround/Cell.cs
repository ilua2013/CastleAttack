using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Button _startFightButton;
    [SerializeField] private List<UnitSpawner> _unitSpawners;

    public Transform TargetPoint => _targetPoint;
    public Button ButtonStartFight => _startFightButton;

    private void OnValidate()
    {
        _startFightButton = FindObjectOfType<CellSpawner>().ButtonStartFight;

        var spawners = GetComponents<UnitSpawner>();
        _unitSpawners.Clear();
        foreach (var item in spawners)
        {
            _unitSpawners.Add(item);
        }
    }
}
