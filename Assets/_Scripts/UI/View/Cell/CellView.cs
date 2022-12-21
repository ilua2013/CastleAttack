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
    private Cell _cell;

    private void OnValidate()
    {
        GetComponent<RectTransform>().localPosition = new Vector3(0, -0.475f, 0);
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _cell = GetComponentInParent<Cell>();
    }

    private void OnEnable()
    {
        _cell.StagedUnit += PlayAnimationSizeDown;
        _cell.UnitMoveToThis += PlayAnimationSizeUp;
    }

    private void OnDisable()
    {
        _cell.StagedUnit -= PlayAnimationSizeDown;
        _cell.UnitMoveToThis -= PlayAnimationSizeUp;
    }

    public void RecolorToWin()
    {
        _animator.Play(ScaleAnimation);
        _image.material = _winColor;
    }

    private void PlayAnimationSizeUp()
    {
        _animator.SetTrigger("ScaleUp");
    }

    private void PlayAnimationSizeDown()
    {
        _animator.SetTrigger("ScaleDown");
    }
}
