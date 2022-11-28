using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardHoverView))]
public class CardSelectVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _vfx;

    private CardHoverView _cardHover;

    private void Awake()
    {
        _cardHover = GetComponent<CardHoverView>();
    }

    private void OnEnable()
    {
        _cardHover.Enter += OnEnter;
        _cardHover.Exit += OnExit;
    }

    private void OnDisable()
    {
        _cardHover.Enter -= OnEnter;
        _cardHover.Exit -= OnExit;
    }

    private void OnEnter(CardHoverView card)
    {
        _vfx.Play();
    }

    private void OnExit(CardHoverView card)
    {
        _vfx.Stop();
    }
}
