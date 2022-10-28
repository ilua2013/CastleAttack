using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSpawner : MonoBehaviour, ICardApplicable
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Button _button;
    [SerializeField] private Transform _targetPoint;

    private LevelSystem _finisher;
    private List<IUnit> _units = new List<IUnit>();

    public Transform SpawnPoint => _spawnPoint;
    public Transform TargetPoint => _targetPoint;
    public Button Button => _button;

    private void OnValidate()
    {
        _spawnPoint = transform;
    }

    private void Awake()
    {
        _finisher = FindObjectOfType<LevelSystem>();
    }

    private void OnEnable()
    {
        _finisher.Wave1Finished += OnFinished;
        _finisher.Wave2Finished += OnFinished;
        _finisher.Wave3Finished += OnFinished;
    }

    private void OnDisable()
    {
        _finisher.Wave1Finished -= OnFinished;
        _finisher.Wave2Finished -= OnFinished;
        _finisher.Wave3Finished -= OnFinished;
    }

    public bool TryApply(Card card, Vector3 place)
    {
        if (card is UnitCard unitCard)
        {
            Unit unit = Instantiate(unitCard.UnitPrefab, SpawnPoint.position, Quaternion.identity);

            unit.Init(card, TargetPoint, Button);
            unit.Deaded += OnUnitDead;

            _units.Add(unit);
            return true;
        }
        return false;
    }

    private void OnFinished()
    {
        foreach (var unit in _units)
        {
            unit.Card.ComeBack();
            unit.ReurnToHand();
        }

        _units.Clear();
    }

    private void OnUnitDead(IMonstr monstr, IUnit unit)
    {
        monstr.Deaded -= OnUnitDead;
        _units.Remove(unit);
    }
}
