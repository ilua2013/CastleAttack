using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Castle : MonoBehaviour
{
    public bool test;
    [SerializeField] private CastleType _type;
    [SerializeField] private Transform _viewMesh;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _reward;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _delayDie;
    [Header("Particle")]
    [SerializeField] private ParticleSystem _fire;
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private ParticleSystem _coins;
    [SerializeField] private GameObject _king;

    private int _currentHealth;

    public int Reward => _reward;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    public CastleType CastleType => _type;

    public event Action Damaged;
    public event Action HealthSeted;
    public event Action Died;

    private void Awake()
    {
        _king.gameObject.SetActive(false);
        _currentHealth = _maxHealth;

        TryEnableParticle();
    }

    private void Update()
    {
        if (test)
        {
            test = false;
            StartCoroutine(TakeDamage(10,1));
        }
    }

    public void SetHealth(int health)
    {
        _currentHealth = health;
        HealthSeted?.Invoke();
    }

    public IEnumerator TakeDamage(int damage, float delay, Action onDie = null)
    {
        yield return new WaitForSeconds(delay);

        _animator.SetTrigger("Hit");
        _currentHealth -= damage;

        if (_currentHealth < 0)
            _currentHealth = 0;

        TryEnableParticle();

        SaveCastle.Health = _currentHealth;
        Damaged?.Invoke();

        if (_currentHealth < 1)
        {
            SaveCastle.CountDead++;

            _fire.Play();

            yield return new WaitForSeconds(_delayDie);

            _smoke.Play();
            _fire.Stop();

            yield return new WaitForSeconds(0.25f);

            Died?.Invoke();
            onDie?.Invoke();

            EnableKing();

            _viewMesh.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.3f);

            _coins.Play();
        }
    }

    private void TryEnableParticle()
    {
        if(_currentHealth <= _maxHealth / 2)
        {
            _fire.Play();
        }
    }

    private void EnableKing()
    {
        _king.gameObject.SetActive(true);
    }
}
