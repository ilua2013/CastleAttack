using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Castle : MonoBehaviour
{
    public bool test;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _takeDamage;
    [SerializeField] private float _delayDamage;
    [SerializeField] private float _delayDie;
    [Header("Particle")]
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private GameObject _king;

    private int _currentHealth;

    public int MaxHealth => _maxHealth;
    public int CurrenHealth => _currentHealth;

    public event Action Damaged;
    public event Action Died;

    private void OnValidate()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Awake()
    {
        if (PlayerPrefs.GetInt("CastleAttack", 0) == 1)
        {
            StartCoroutine(TakeDamage(_delayDamage));
            PlayerPrefs.SetInt("CastleAttack", 0);
        }
        _king.gameObject.SetActive(false);
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (test)
        {
            test = false;
            StartCoroutine(TakeDamage(_delayDamage));
        }
    }

    private IEnumerator TakeDamage(float delay)
    {
        yield return new WaitForSeconds(delay);

        _animator.SetTrigger("Hit");
        _currentHealth -= _takeDamage;

        Damaged?.Invoke();

        if (_currentHealth < 1)
        {
            _fire.Play();
            yield return new WaitForSeconds(_delayDie);

            _smoke.Play();
            _fire.Stop();
            yield return new WaitForSeconds(0.2f);

            Died?.Invoke();
            EnableKing();
            _meshRenderer.enabled = false;
        }
    }

    private void EnableKing()
    {
        _king.gameObject.SetActive(true);
    }
}
