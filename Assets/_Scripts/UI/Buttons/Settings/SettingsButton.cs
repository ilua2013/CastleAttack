using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SettingsButton : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private bool _isOpened;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);

    }

    private void OnClick()
    {
        if (_isOpened)
            _animator.Play("Close");
        else
            _animator.Play("Open");

        _isOpened = !_isOpened;
    }
}
