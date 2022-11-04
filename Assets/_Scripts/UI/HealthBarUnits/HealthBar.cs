using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float LerpTime = 1f;
    private const float LerpError = 0.01f;

    [SerializeField] private Fighter _fighter;
    [SerializeField] private Slider _bar;

    private void OnEnable()
    {
        _fighter.Damaged += OnHealthChanged;
        _fighter.Healed += OnHealthChanged;
    }

    private void OnDisable()
    {
        _fighter.Damaged -= OnHealthChanged;
        _fighter.Healed -= OnHealthChanged;
    }

    private void OnHealthChanged(int amount)
    {
        float remain = (float)_fighter.Health / _fighter.MaxHealth;
        StartCoroutine(LerpValue(remain, LerpTime));
    }

    private IEnumerator LerpValue(float to, float time)
    {
        while (MathF.Abs(_bar.value - to) > LerpError)
        {
            _bar.value = Mathf.MoveTowards(_bar.value, to, time * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
