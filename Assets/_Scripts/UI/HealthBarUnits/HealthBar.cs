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

    [SerializeField] private MonoBehaviour _iUnit;
    [SerializeField] private TMP_Text _textHealth;
    [SerializeField] private Slider _bar;
    [SerializeField] private Vector3 _posistionViewWizzard;

    private Vector3 _startPos;

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
        _startPos = transform.localPosition;

        if (_unit.Fighter.Health == 0)
            _bar.value = _unit.Fighter.MaxHealth;
        else
            _bar.value = _unit.Fighter.Health;
    }

    private void OnEnable()
    {
        _unit.Fighter.Died += Destroy;
        _unit.Inited += StartOnHealthChanged;
        _unit.Fighter.Damaged += OnHealthChanged;
        _unit.Fighter.Healed += OnHealthChanged;

        if(_unit is UnitFriend unitFriend)
        {
            unitFriend.RotatedToBattle += PutDefaultPosition;
            unitFriend.RotatedToWizzard += PutPositionViewWizzard;
        }
    }

    private void OnDisable()
    {
        _unit.Fighter.Died -= Destroy;
        _unit.Inited -= StartOnHealthChanged;
        _unit.Fighter.Damaged -= OnHealthChanged;
        _unit.Fighter.Healed -= OnHealthChanged;

        if (_unit is UnitFriend unitFriend)
        {
            unitFriend.RotatedToBattle -= PutDefaultPosition;
            unitFriend.RotatedToWizzard -= PutPositionViewWizzard;
        }
    }

    private void PutPositionViewWizzard()
    {
        transform.localPosition = _posistionViewWizzard;
    }

    private void PutDefaultPosition()
    {
        transform.localPosition = _startPos;
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

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void SetText() => _textHealth.text = _unit.Fighter.Health.ToString()/* + "/" + _unit.Fighter.MaxHealth*/;
}
