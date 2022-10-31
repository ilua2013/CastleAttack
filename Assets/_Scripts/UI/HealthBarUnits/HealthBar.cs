using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Fighter _fighter;
    [SerializeField] private Slider _bar;

    private void OnEnable()
    {
        _fighter.Damaged += OnTakeDamage;
        _fighter.Healed += OnRecovery;
    }

    private void OnDisable()
    {
        _fighter.Damaged -= OnTakeDamage;
        _fighter.Healed -= OnRecovery;
    }

    private void OnTakeDamage(int damage)
    {
        float remain = (float)_fighter.Health / _fighter.MaxHealth;
        StartCoroutine(LerpValue(remain, 1f));
    }

    private void OnRecovery(int amount)
    {
        float remain = (float)_fighter.Health / _fighter.MaxHealth;

        StartCoroutine(LerpValue(remain, 1f));
    }

    private IEnumerator LerpValue(float to, float time)
    {
        while (MathF.Abs(_bar.value - to) > 0.01f)
        {
            _bar.value = Mathf.MoveTowards(_bar.value, to, time * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
