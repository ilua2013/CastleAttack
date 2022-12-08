using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageView : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _iUnit;
    [SerializeField] private TMP_Text _textDamage;
    [SerializeField] private RectTransform _textDamageTransform;   
    [SerializeField] private float _timeViewDamage;

    private int _healt;

    private IUnit _unit => (IUnit)_iUnit;

    private void OnValidate()
    {
        _iUnit = (MonoBehaviour)GetComponentInParent<IUnit>();
        if (_iUnit is IUnit == false)
            _iUnit = null;
    }

    private void Awake()
    {
        _iUnit = (MonoBehaviour)GetComponentInParent<IUnit>();
        _healt = _unit.Fighter.MaxHealth;        
    }

    private void OnEnable()
    {        
        _unit.Fighter.Damaged += DamageViewText;
        _unit.Fighter.EffectDied += DisableText;
    }

    private void OnDisable()
    {       
        _unit.Fighter.Damaged -= DamageViewText;
        _unit.Fighter.EffectDied -= DisableText;
    }

    private void DamageViewText(int damage)
    {       
        _textDamage.gameObject.SetActive(true);
        if(_healt>=damage)
        _textDamage.text = $"{-damage}";
        else
            _textDamage.text = $"{-_healt}";
        _healt -= damage;
        StartCoroutine(TextDown());
    }

    private IEnumerator TextDown()
    {
        
            yield return new WaitForSeconds(_timeViewDamage);
        
        _textDamage.gameObject.SetActive(false);
    }

    private void DisableText()
    {
        _textDamage.gameObject.SetActive(false);
    }
}
