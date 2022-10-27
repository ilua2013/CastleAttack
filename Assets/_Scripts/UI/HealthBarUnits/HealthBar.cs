using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _unitDamageable;
    [SerializeField] private Slider _bar;

    private IDamageable _damageable;

    private void Awake()
    {
        if (_unitDamageable is IDamageable)
            _damageable = _unitDamageable as IDamageable;
        else
            _unitDamageable = null;
    }

    private void OnEnable()
    {
        _damageable.Damaged += OnTakeDamage;
        _damageable.Healed += OnRecovery;
    }

    private void OnDisable()
    {
        _damageable.Damaged -= OnTakeDamage;
        _damageable.Healed -= OnRecovery;
    }

    private void OnTakeDamage(IMonstr monstr, int damage)
    {
        float remain = (float)monstr.Health / monstr.MaxHealth;
        StartCoroutine(LerpValue(remain, 1f));
    }

    private void OnRecovery(IMonstr monstr, int amount)
    {
        float remain = (float)monstr.Health / monstr.MaxHealth;
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
