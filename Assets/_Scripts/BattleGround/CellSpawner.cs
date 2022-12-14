using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSpawner : MonoBehaviour
{
    [SerializeField] private Transform _pointSpawn;
    [SerializeField] private Cell _cell;
    [Header("FriendCell")]
    [SerializeField] private int _countRowFriend;
    [SerializeField] private Cell _cellFriend;
    [Header("EnemyCell")]
    [SerializeField] private int _countRowEnemy;
    [SerializeField] private Cell _cellEnemy;
    [Header("Grid")]
    [SerializeField] private Vector2 _grid;
    [SerializeField] private Vector2 _offset;
    [Header("OnValidate")]
    [SerializeField] private bool _activeSpawnCell;

    private List<Cell> _cells = new List<Cell>();

    private void OnValidate()
    {
        if(_activeSpawnCell)
        {
            if (_cells.Count > 0)
                _cells.Clear();

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

            _cells.Clear();
        }
    }

    public void SpawnCell()
    {
        Vector3 offset = Vector3.zero;
        Cell spawn = _cell;

        for (int i = 0; i < _grid.x; i++)
        {
            if (_countRowFriend > i)
                spawn = _cellFriend;
            else if (_grid.x - _countRowEnemy <= i)
                spawn = _cellEnemy;
            else
                spawn = _cell;

            for (int y = 0; y < _grid.y; y++)
            {
                Cell cell = Instantiate(spawn, _pointSpawn.position + offset, Quaternion.identity, transform);

                if (i == 0)
                    cell.SetType(CellIs.Lower);
                else if (i + 1 == _grid.x)
                    cell.SetType(CellIs.Higher);
                else
                    cell.SetType(CellIs.Default);

                _cells.Add(cell);
                offset.x += _offset.x;
            }

            offset.x = 0;
            offset.z += _offset.y;
        }
    }
}
