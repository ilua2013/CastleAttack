using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Fighter _unitDamageable;
    [SerializeField] private Slider _bar;

    //private IDamageable _damageable;

    public void Init(Fighter unitHealt)
    {
        _unitDamageable = unitHealt;
    }

    //private void Awake()
    //{
    //    if (_unitDamageable is IDamageable)
    //        _damageable = _unitDamageable as IDamageable;
    //    else
    //        _unitDamageable = null;
    //}

    private void OnEnable()
    {
        _unitDamageable.Damaged += OnTakeDamage;
        //_damageable.Healed += OnRecovery;
    }

    private void OnDisable()
    {
        _unitDamageable.Damaged -= OnTakeDamage;
        //_damageable.Healed -= OnRecovery;
    }

    private void OnTakeDamage(int healt)
    {
        
        StartCoroutine(LerpValue(healt, 1f));
    }

    //private void OnRecovery( int healt)
    //{
        
    //    StartCoroutine(LerpValue(remain, 1f));
    //}

    private IEnumerator LerpValue(float to, float time)
    {
        while (MathF.Abs(_bar.value - to) > 0.01f)
        {
            _bar.value = Mathf.MoveTowards(_bar.value, to, time * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
