using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Castle : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _takeDamage;
    [SerializeField] private float _delayDamage;

    private int _currentHealth;

    public int MaxHealth => _maxHealth;
    public int CurrenHealth => _currentHealth;

    public event Action Damaged;
    public event Action Died;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("CastleAttack", 0) == 1)
            StartCoroutine(TakeDamage(_delayDamage));

        _currentHealth = _maxHealth;
    }

    private IEnumerator TakeDamage(float delay)
    {
        yield return new WaitForSeconds(delay);

        _animator.SetTrigger("Hit");
        _currentHealth -= _takeDamage;

        Damaged?.Invoke();

        if (_currentHealth < 1)
            Died?.Invoke();
    }
}
