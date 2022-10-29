using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FightSystem : MonoBehaviour
{
    [SerializeField] private Button _buttonStartFight;
    [SerializeField] private UnitSpawner[] _spawners;
    [SerializeField] private float _delayBeetwenStep;

    public List<UnitStep> _unitsSpawned = new List<UnitStep>();

    private void OnValidate()
    {
        _spawners = FindObjectsOfType<UnitSpawner>();
    }

    private void OnEnable()
    {
        _buttonStartFight.onClick.AddListener(StartUnitsDoStep);

        foreach (var spawner in _spawners)
            spawner.SpawnedUnit += AddUnit;
    }

    private void OnDisable()
    {
        _buttonStartFight.onClick.RemoveListener(StartUnitsDoStep);

        foreach (var spawner in _spawners)
            spawner.SpawnedUnit -= AddUnit;
    }

    private void StartUnitsDoStep()
    {
        StartCoroutine(UnitsDoStep());
    }

    private IEnumerator UnitsDoStep()
    {
        while (CheckHaveStep())
        {
            foreach (var item in _unitsSpawned)
                item.DoStep();

            yield return new WaitForSeconds(_delayBeetwenStep);
        }

        foreach (var item in _unitsSpawned)
            item.UpdateStep();
    }

    private void AddUnit(UnitStep unit)
    {
        if (_unitsSpawned.Contains(unit) == false)
        {
            _unitsSpawned.Add(unit);
            unit.Fighter.Died += RemoveUnit;
        }
    }

    private void RemoveUnit(Fighter fighter)
    {
        if (_unitsSpawned.Contains(fighter.GetComponent<UnitStep>()) == true)
            _unitsSpawned.Remove(fighter.GetComponent<UnitStep>());
    }

    private bool CheckHaveStep()
    {
        foreach (var item in _unitsSpawned)
        {
            if (item.CurrentStep > 0)
                return true;
        }

        return false;
    }
}
