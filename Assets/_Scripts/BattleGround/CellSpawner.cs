using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSpawner : MonoBehaviour
{
    [SerializeField] private Transform _pointSpawn;
    [SerializeField] private Cell _cell;
    [SerializeField] private float _offsetMainTarget;
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

    public List<Cell> Cells => _cells;
    public int Rows => (int)_grid.x;

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

            if (i == 0)
                SpawnCellWizzard();

            //if (i + 1 == _grid.x)
            //    SpawnCellBoss();

            offset.x = 0;
            offset.z += _offset.y;
        }
    }

    private void SpawnCellWizzard()
    {
        Transform firtsCell = _cells[0].transform;
        Transform lastCell = _cells[_cells.Count - 1].transform;

        Vector3 spawnPoint = (firtsCell.localPosition + lastCell.localPosition) / 2f;

        Cell cell = Instantiate(_cellFriend, Vector3.zero, Quaternion.identity, transform);
        cell.transform.localPosition = spawnPoint + new Vector3(0, 0, -_offsetMainTarget);
        cell.SetType(CellIs.Wizzard);
        cell.gameObject.name += " Wizzard";

        cell.GetComponentInChildren<CellView>().gameObject.SetActive(false);

        foreach (var item in _cells)
            item.SetCell(cell, CellNeighbor.Bot);
    }

    private void SpawnCellBoss()
    {
        Transform firtsCell = _cells[_cells.Count - (int)_grid.y].transform;
        Transform lastCell = _cells[_cells.Count - 1].transform;

        Vector3 spawnPoint = (firtsCell.localPosition + lastCell.localPosition) / 2f;

        Cell cell = Instantiate(_cellEnemy, Vector3.zero, Quaternion.identity, transform);
        cell.transform.localPosition = spawnPoint + new Vector3(0, 0, _offsetMainTarget);
        cell.SetType(CellIs.Boss);
        cell.gameObject.name += " Boss";

        for (int i = 0; i < _grid.y; i++)
            _cells[_cells.Count - 1 - i].SetCell(cell, CellNeighbor.Top);
    }
}
