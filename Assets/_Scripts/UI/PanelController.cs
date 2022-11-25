using UnityEngine;
using UnityEngine.UI;
using System;

public class PanelController : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button[] _closeButtons;
    [SerializeField] private GameObject _background;

    public event Action Opened;
    public event Action Closed;

    public bool IsOpened { get; private set; }

    private void OnEnable()
    {
        _openButton.onClick.AddListener(Open);

        foreach (Button button in _closeButtons)
            button.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(Open);

        foreach (Button button in _closeButtons)
            button.onClick.RemoveListener(Close);
    }

    private void Open()
    {
        _background.SetActive(true);

        IsOpened = true;
        Opened?.Invoke();
    }

    private void Close()
    {
        _background.SetActive(false);

        IsOpened = false;
        Closed?.Invoke();
    }
}
