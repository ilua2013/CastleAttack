using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Castle _castle;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _textCountDead;
    [SerializeField] private float _speedChangeValue;

    private Coroutine _animationSlowChangeValue;

    private void OnValidate()
    {
        _castle = GetComponentInParent<Castle>();
    }

    private void Awake()
    {
        if (_castle == null)
            _castle = GetComponentInParent<Castle>();
    }

    private void Start()
    {
        _text.text = _castle.CurrentHealth.ToString();
        _slider.value = (float)_castle.CurrentHealth / _castle.MaxHealth;
        _textCountDead.text = SaveCastle.CountDead.ToString();
    }

    private void OnEnable()
    {
        _castle.Damaged += UpdateViewBar;
        _castle.Died += Disable;
        _castle.HealthSeted += UpdateView;
        _slider.value = (float)_castle.CurrentHealth / _castle.MaxHealth;
    }

    private void OnDisable()
    {
        _castle.Damaged -= UpdateViewBar;
        _castle.Died -= Disable;
        _castle.HealthSeted -= UpdateView;
    }

    private void UpdateView()
    {
        _text.text = _castle.CurrentHealth.ToString();
        _slider.value = _castle.CurrentHealth / _castle.MaxHealth;
    }

    private void UpdateViewBar()
    {
        if (_animationSlowChangeValue == null)
            StartCoroutine(AnimationSlowChangeValue((float)_castle.CurrentHealth / _castle.MaxHealth));
        else
        {
            StopCoroutine(_animationSlowChangeValue);
            StartCoroutine(AnimationSlowChangeValue((float)_castle.CurrentHealth / _castle.MaxHealth));
        }

        _text.text = _castle.CurrentHealth.ToString();
    }

    private IEnumerator AnimationSlowChangeValue(float to)
    {
        while(_slider.value != to)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, to, _speedChangeValue * Time.deltaTime);
            yield return null;
        }

        _animationSlowChangeValue = null;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
