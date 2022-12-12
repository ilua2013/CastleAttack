using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightUnit : MonoBehaviour
{
    [SerializeField] private GameObject _arrowHighlightUnit;
    private IUnit _unit;

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
        //if (isSteped == true)
        //    //_highlightUnit.Play();
        //    
        if(_arrowHighlightUnit!=null)
            _arrowHighlightUnit.gameObject.SetActive(isSteped);
        //else
        //    _highlightUnit.Stop();
    }
}
