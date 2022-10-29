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
    private bool _isMove = false;

    private void Start()
    {
        _currentCell = _initCell;
        _topCell = _currentCell.Top;
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
        _isMove = true;
        _topCell = _currentCell.Top;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Cell triggered))
        {
            _currentCell = triggered;
            //_topCell = _currentCell.Top;

        }
    }

    private void Update()
    {
        if(_isMove == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _topCell.transform.position, _moveSpeed * Time.deltaTime);
        }
        if (transform.position == _topCell.transform.position)
        {
            _isMove = false;           
        }


    }




}
