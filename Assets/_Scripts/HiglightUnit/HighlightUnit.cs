using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightUnit : MonoBehaviour
{
    [SerializeField] private GameObject _arrowHighlightUnit;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _delay;

    private IUnit _unit;
    private bool _isSteped;

    private void Awake()
    {
        _arrowHighlightUnit.gameObject.SetActive(false);
        _unit = GetComponent<IUnit>();
    }

    private void OnEnable()
    {
        _unit.UnitSteped += SwitchHiglighting; 
    }
    private void OnDisable()
    {
        _unit.UnitSteped -= SwitchHiglighting;
    }

    private void SwitchHiglighting(bool isSteped)
    {     
       
        if(isSteped == false)
        {
            _animator.SetBool("isDecrease", true);
            StartCoroutine(delayArrow());
        }
        else
        {
            if (_arrowHighlightUnit != null)
                _arrowHighlightUnit.gameObject.SetActive(true);
        }      
      
    }

    private IEnumerator delayArrow()
    {

        yield return new WaitForSeconds(_delay);
        if (_arrowHighlightUnit != null)
            _arrowHighlightUnit.gameObject.SetActive(false);
    } 
}
