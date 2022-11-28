using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StepCounterUI : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _iUnit;
    [SerializeField] private TMP_Text _text;

    private IUnit _unit => (IUnit)_iUnit;

    private void OnValidate()
    {
        _iUnit = (MonoBehaviour)GetComponentInParent<IUnit>();
        if (_iUnit is IUnit == false)
            _iUnit = null;
    }

    private void Awake()
    {
        _iUnit = (MonoBehaviour)GetComponentInParent<IUnit>();
    }

    private void OnEnable()
    {
        _unit.StepChanged += OnStepChanged;
        SetCountStep(_unit.MaxStep);
    }

    private void OnDisable()
    {
        _unit.StepChanged -= OnStepChanged;
    }

    private void OnStepChanged()
    {
        SetCountStep(_unit.CurrentStep);
    }

    private void SetCountStep(int count)
    {
        _text.text = count.ToString();
    }
}
