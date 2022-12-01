using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ExpirienceUI : MonoBehaviour
{
    [SerializeField] private UnitExperience _expirience;
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _text;

    private string _startText;

    private void OnValidate()
    {
        _expirience = GetComponentInParent<UnitExperience>();
    }

    private void Awake()
    {
        DisableIf();
        _expirience = GetComponentInParent<UnitExperience>();
        _startText = _text.text;
        _slider.value = 0;
        if(_expirience != null)
        _slider.maxValue = _expirience.Thresold;
    }

    private void OnEnable()
    {
        if (_expirience == null)
            return;
            _expirience.LevelUp += OnLevelUp;
        _expirience.Up += OnKill;
    }

    private void OnDisable()
    {
        if (_expirience == null)
            return;
        _expirience.LevelUp -= OnLevelUp;
        _expirience.Up -= OnKill;
    }

    private void OnLevelUp(int amount = 0)
    {
        _slider.value = 0;
        _slider.maxValue = _expirience.Thresold;
        SetText();
    }

    private void OnKill(int amount = 0)
    {
        float remain = (float)_expirience.CurrentKill / _expirience.Thresold;
        StartCoroutine(LerpValue(remain, 1f));
    }

    private IEnumerator LerpValue(float to, float time)
    {
        while (MathF.Abs(_slider.value - to) > 0.001f)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, to, time * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void SetText()
    {
        _text.text = _expirience.Level.ToString();
    }

    private void DisableIf()
    {
        var unit = GetComponentInParent<UnitFriend>();

        if (unit.Fighter.FighterType == FighterType.MainWizzard)
        {
            gameObject.SetActive(false);
        }
    }
}
