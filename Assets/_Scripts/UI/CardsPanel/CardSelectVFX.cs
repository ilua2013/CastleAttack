using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardHoverView))]
public class CardSelectVFX : MonoBehaviour
{
    private ParticleSystem _vfxSelect;
    private CardHoverView _cardHover;

    private void Awake()
    {
        _cardHover = GetComponent<CardHoverView>();
        _vfxSelect = GetComponentInChildren<ParticleSystem>();
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
        _vfxSelect.Play();
    }

    private void OnExit(CardHoverView card)
    {
        _vfxSelect.Stop();
    }
}
