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
    [SerializeField] private float _speedChangeValue;

    private Coroutine _animationSlowChangeValue;

    private void OnValidate()
    {
        _castle = GetComponentInParent<Castle>();
    }

    private void Start()
    {
        _text.text = _castle.CurrenHealth.ToString();
        _slider.value = _castle.CurrenHealth / _castle.MaxHealth;
    }

    private void OnEnable()
    {
        _castle.Damaged += UpdateViewBar;
        _castle.Died += Disable;
        _slider.value = _castle.CurrenHealth / _castle.MaxHealth;
    }

    private void OnDisable()
    {
        _castle.Damaged -= UpdateViewBar;
        _castle.Died -= Disable;
    }

    private void UpdateViewBar()
    {
        if (_animationSlowChangeValue == null)
            StartCoroutine(AnimationSlowChangeValue(_castle.CurrenHealth / _castle.MaxHealth));
        else
        {
            StopCoroutine(_animationSlowChangeValue);
            StartCoroutine(AnimationSlowChangeValue(_castle.CurrenHealth / _castle.MaxHealth));
        }

        _text.text = _castle.CurrenHealth.ToString();
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
