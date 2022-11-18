using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private const float LerpTime = 1f;
    private const float LerpError = 0.01f;

    [SerializeField] private TMP_Text _textHealth;
    [SerializeField] private Slider _bar;

    private IUnit _unit;

    private void Awake()
    {
        _unit = GetComponentInParent<IUnit>();
    }

    private void OnEnable()
    {
        _unit.Inited += StartOnHealthChanged;
        _unit.Fighter.Damaged += OnHealthChanged;
        _unit.Fighter.Healed += OnHealthChanged;
    }

    private void OnDisable()
    {
        _unit.Inited -= StartOnHealthChanged;
        _unit.Fighter.Damaged -= OnHealthChanged;
        _unit.Fighter.Healed -= OnHealthChanged;
    }

    private void StartOnHealthChanged() => OnHealthChanged(0);

    private void OnHealthChanged(int amount)
    {
        float remain = (float)_unit.Fighter.Health / _unit.Fighter.MaxHealth;
        StartCoroutine(LerpValue(remain, LerpTime));
        SetText();
    }

    private IEnumerator LerpValue(float to, float time)
    {
        while (MathF.Abs(_bar.value - to) > LerpError)
        {
            _bar.value = Mathf.MoveTowards(_bar.value, to, time * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void SetText() => _textHealth.text = _unit.Fighter.Health + "/" + _unit.Fighter.MaxHealth;
}
