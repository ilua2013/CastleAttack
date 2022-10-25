using System.Collections.Generic;
using UnityEngine;

public class CellSpawner : MonoBehaviour
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private Transform _pointSpawn;
    [SerializeField] private Cell _cell;
    [SerializeField] private Vector2 _grid;
    [SerializeField] private Vector2 _offset;
    [Header("SpawnCell")]
    [SerializeField] private bool _activeSpawnCell;

    private List<Cell> _cells = new List<Cell>();

    private void OnValidate()
    {
        if(_activeSpawnCell && _cells.Count == 0)
        {
            _activeSpawnCell = false;
            SpawnCell();
        }
    }

    public void Clear()
    {
        if (_cells.Count > 0)
        {
            foreach (var item in _cells)
                Destroy(item.gameObject);
        }
        _cells.Clear();
    }

    public void SpawnCell()
    {
        Vector3 offset = Vector3.zero;

        for (int i = 0; i < _grid.x; i++)
        {
            for (int y = 0; y < _grid.y; y++)
            {
                _cells.Add(Instantiate(_cell, _pointSpawn.position + offset, Quaternion.identity, transform));
                offset.x += _offset.x;
            }
            offset.x = 0;
            offset.z += _offset.y;
        }
    }
}
