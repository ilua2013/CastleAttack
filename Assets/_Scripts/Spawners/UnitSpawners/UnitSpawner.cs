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

    public bool TryApply(CardDescription card, Vector3 place)
    {
        return TryApplySpell(card, place);
    }

    protected abstract bool TryApplySpell(CardDescription card, Vector3 place);
}
