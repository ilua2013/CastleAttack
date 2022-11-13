using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextEffects : MonoBehaviour
{
    //[SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private Color _color;
    [SerializeField] private Image _image;
    [SerializeField] private Color _imageColor;

    private Color _defaultColor;
    private Color _defaultImageColor;

    private void Start()
    {
       _defaultColor = _text.color;        
        _text.color = _color;
        //_defaultImageColor = _image.color;
        //_image.color = _imageColor;
        
    }

    private void Update()
    {
        ChangeColorText();
    }

    private void ChangeColorText()
    {
        _text.color = Color.Lerp(_color, _defaultColor, Mathf.Abs(Mathf.Sin(Time.time/2)));
    }


}
