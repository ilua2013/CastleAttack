using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

[RequireComponent(typeof(Inventory))]
public class PanelController : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private GameObject[] _childs;

    public event Action Opened;
    public event Action Closed;

    private void OnEnable()
    {
        _openButton.onClick.AddListener(Open);
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(Open);
        _closeButton.onClick.RemoveListener(Close);
    }

    private void Open()
    {
        foreach (var child in _childs)
            child.SetActive(true);

        Opened?.Invoke();
    }

    private void Close()
    {
        foreach (var child in _childs)
            child.SetActive(false);

        Closed?.Invoke();
    }
}
