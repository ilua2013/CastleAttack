using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Card))]
public class SoundEffectCard : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer _settings;

    private Card _card;
    private CardHoverView _cardHover;

    private void Awake()
    {
        _card = GetComponent<Card>();
        _cardHover = GetComponent<CardHoverView>();
    }

    private void OnEnable()
    {
        _card.Clicked += OnClicked;
        _cardHover.CancelDrop += OnComeBack;
    }

    private void OnDisable()
    {
        _card.Clicked -= OnClicked;
        _cardHover.CancelDrop -= OnComeBack;
    }

    private void OnClicked(PointerEventData eventData, Card card)
    {
        _settings.Play(SoundEffectType.CardClick);
    }

    private void OnComeBack(CardHoverView cardHover)
    {
        _settings.Play(SoundEffectType.CardComeBack);
    }

    private IEnumerator PlayWithDelay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);

        action?.Invoke();
    }
}
