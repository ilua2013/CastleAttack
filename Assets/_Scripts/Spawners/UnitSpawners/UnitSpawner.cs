using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnitSpawner : MonoBehaviour, ICardApplicable
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Button _button;
    [SerializeField] private Transform _targetPoint;

    public Transform SpawnPoint => _spawnPoint;
    public Transform TargetPoint => _targetPoint;
    public Button Button => _button;

    private void OnValidate()
    {
        _spawnPoint = transform;
        var cell = GetComponent<Cell>();
        _button = cell.ButtonStartFight;
        _targetPoint = cell.TargetPoint;
    }

    public bool TryApply(Card card, Vector3 place)
    {
        return TryApplyUnit(card, place);
    }

    protected abstract bool TryApplyUnit(Card card, Vector3 place);
}
