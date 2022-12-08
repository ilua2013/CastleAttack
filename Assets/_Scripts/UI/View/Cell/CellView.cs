using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    private const string ScaleAnimation = "Scale";

    [SerializeField] private Image _image;
    [SerializeField] private Material _winColor;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void RecolorToWin()
    {
        _animator.Play(ScaleAnimation);
        _image.material = _winColor;
    }
}
