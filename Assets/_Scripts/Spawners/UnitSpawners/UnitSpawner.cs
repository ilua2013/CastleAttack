using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UnitSpawner : MonoBehaviour, ICardApplicable
{
    [SerializeField] private Transform _spawnPoint;
    
    private Button _button;
    private Transform _targetPoint;

    public Transform SpawnPoint => _spawnPoint;
    public Transform TargetPoint => _targetPoint;
    public Button Button => _button;

    public void Init(Transform targetPoint, Button button)
    {
        _targetPoint = targetPoint;
        _button = button;
    }

    public bool TryApply(Card card, Vector3 place)
    {
        return TryApplyUnit(card, place);
    }

    protected abstract bool TryApplyUnit(Card card, Vector3 place);
}
