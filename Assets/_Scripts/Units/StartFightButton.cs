using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartFightButton : MonoBehaviour
{
    [SerializeField] private Button _buttonFight;
    [SerializeField] private Button _buttonSkipStep;
    [SerializeField] private Image _imageButton;
    [SerializeField] private BattleSystem _fightSystem;
    [SerializeField] private CellSpawner _cellSpawner;

    private SpellSpawner[] _speelSpawners;
    private UnitSpawner[] _unitSpawners;

    public Button Button => _buttonFight;

    private void OnValidate()
    {
        _fightSystem = FindObjectOfType<BattleSystem>();
        _cellSpawner = FindObjectOfType<CellSpawner>();
    }

    private void Awake()
    {
        _speelSpawners = _cellSpawner.GetComponentsInChildren<SpellSpawner>();
        _unitSpawners = _cellSpawner.GetComponentsInChildren<UnitSpawner>();
    }

    private void OnEnable()
    {
        _fightSystem.StepFinished += EnableButton;
        _fightSystem.BattleStarted += DisableButton;

        foreach (var item in _speelSpawners)
            item.Cast += DisableButtonSkipStep;

        foreach (var item in _unitSpawners)
            item.SpawnedUnit += DisableButtonSkipStep;
    }
    private void OnDisable()
    {
        _fightSystem.StepFinished -= EnableButton;
        _fightSystem.BattleStarted -= DisableButton;

        foreach (var item in _speelSpawners)
            item.Cast -= DisableButtonSkipStep;

        foreach (var item in _unitSpawners)
            item.SpawnedUnit -= DisableButtonSkipStep;
    }

    private void EnableButton()
    {
        _buttonFight.enabled = true;
        EnableButtonSkipStep();
        _imageButton.color = Color.yellow;
    }

    private void DisableButton()
    {
        DisableButtonSkipStep();
        _buttonFight.enabled = false;
        _imageButton.color = Color.gray;
    }

    private void DisableButtonSkipStep()
    {
        _buttonSkipStep.interactable = false;
    }

    private void EnableButtonSkipStep()
    {
        _buttonSkipStep.interactable = true;
    }
}
