using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FighterTypeUI : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private MonoBehaviour _iUnit;
    [SerializeField] private FighterTypeImage[] _fighterTypes = new FighterTypeImage[] { };

    private IUnit _unit => (IUnit)_iUnit;

    [Serializable]
    private class FighterTypeImage
    {
        public FighterType FighterType;
        public Sprite Sprite;
    }

    private void OnValidate()
    {
        _iUnit = (MonoBehaviour)GetComponentInParent<IUnit>();
        if (_iUnit is IUnit == false)
        {
            _iUnit = null;
            return;
        }

        foreach (var item in _fighterTypes)
        {
            if (item.FighterType == _unit.Fighter.FighterType)
            {
                _image.sprite = item.Sprite;
                return;
            }
        }
        gameObject.SetActive(false);
    }
}
