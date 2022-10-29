using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNewUnit : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    [SerializeField] private Cell _initCell;
    [SerializeField] private FightSystem _fightSystem;

    private Cell _topCell;
    private Cell _currentCell;

    private void Start()
    {
        _currentCell = _initCell;
        _topCell = _initCell;
    }

    private void OnEnable()
    {
        _fightSystem.Moved += AllowMovement;
    }

    private void OnDisable()
    {
        _fightSystem.Moved -= AllowMovement;
    }

    private void AllowMovement()
    {
        _topCell = _currentCell.Top;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cell triggered))
        {
            _currentCell = triggered;
        }
    }

    private void Update()
    {
        if (transform.position != _topCell.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, _topCell.transform.position, _moveSpeed * Time.deltaTime);
        }
    }
}
