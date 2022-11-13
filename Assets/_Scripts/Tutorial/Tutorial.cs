using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Color _color;   
    [SerializeField] private CardsSelection _cardsSelection;
    [SerializeField] private ParticleSystem[] _particleSystemSelectStars;
    [SerializeField] private float _secondEffect;
    [SerializeField] private ParticleSystem[] _particleDrawCard;
    [SerializeField] private ParticleSystem[] _particleStartButton;
    [SerializeField] private Button _startFightButton;
    [SerializeField] private GameObject _canvasTutorial;
    [SerializeField] private CardsHand _cardsHand;

    private Color _defaultColor;

    private void OnEnable()
    {
        _cardsSelection.CardSelected += SelectionEffect;
        _cardsHand.Spawned += CardSpawned;
    }

    private void OnDisable()
    {
        _cardsSelection.CardSelected -= SelectionEffect;
        _cardsHand.Spawned -= CardSpawned;
    }

    private void Start()
    {
       _defaultColor = _text.color;        
        _text.color = _color;
        ParticleStop(_particleSystemSelectStars);
        ParticleStop(_particleDrawCard);
        _startFightButton.gameObject.SetActive(false);
        _canvasTutorial.SetActive(false);
        ParticleStop(_particleStartButton);
    }

    private void Update()
    {
        ChangeColorText();        
    }

    private void ChangeColorText()
    {
        _text.color = Color.Lerp(_color, _defaultColor, Mathf.Abs(Mathf.Sin(Time.time/2)));
    }

    private void SelectionEffect(Card card)
    {
        ParticlePlay(_particleSystemSelectStars);
        StartCoroutine(PlayStarEffect());
        ParticlePlay(_particleDrawCard);
        _canvasTutorial.SetActive(true);
    }

    private void ParticleStop(ParticleSystem [] particleSystems)
    {
        foreach (var particle in particleSystems)
        {
            particle.Stop();
        }
    }

    private void ParticlePlay(ParticleSystem[] particleSystems)
    {
        foreach (var particle in particleSystems)
        {
            particle.Play();
        }
    }

    private IEnumerator PlayStarEffect()
    {
        WaitForSeconds _forSeconds = new WaitForSeconds(_secondEffect);
        yield return _forSeconds;
        ParticleStop(_particleSystemSelectStars);
        StopCoroutine(PlayStarEffect());
    }

    private void CardSpawned(UnitFriend unitFriend)
    {
        ParticlePlay(_particleSystemSelectStars);
        StartCoroutine(PlayStarEffect());
        _canvasTutorial.SetActive(false);
        ParticleStop(_particleDrawCard);
        _startFightButton.gameObject.SetActive(true);
        ParticlePlay(_particleStartButton);
    }
}
