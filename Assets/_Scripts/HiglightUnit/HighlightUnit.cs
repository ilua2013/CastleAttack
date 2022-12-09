using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightUnit : MonoBehaviour
{
    [SerializeField] private ParticleSystem _highlightUnit;
    private IUnit _unit;

    private void Awake()
    {
        _highlightUnit.gameObject.SetActive(false);
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
        Debug.Log(isSteped);
        //if (isSteped == true)
        //    //_highlightUnit.Play();        
            _highlightUnit.gameObject.SetActive(isSteped);
        //else
        //    _highlightUnit.Stop();
    }
}
