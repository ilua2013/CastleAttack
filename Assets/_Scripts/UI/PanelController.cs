using UnityEngine;
using UnityEngine.UI;
using System;

public class PanelController : MonoBehaviour
{
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButtonVertical;
    [SerializeField] private Button _closeButtonHorizontal;
    [SerializeField] private GameObject _background;

    public event Action Opened;
    public event Action Closed;

    public bool IsOpened { get; private set; }

    private void OnEnable()
    {
        Open();
        _openButton.onClick.AddListener(Open);

        _closeButtonVertical.onClick.AddListener(VerticalClose);
        _closeButtonHorizontal.onClick.AddListener(HorizontalClose);
    }

    private void OnDisable()
    {
        _openButton.onClick.RemoveListener(Open);

        _closeButtonVertical.onClick.RemoveListener(VerticalClose);
        _closeButtonHorizontal.onClick.RemoveListener(HorizontalClose);
    }

    private void Open()
    {
        _background.SetActive(true);

        IsOpened = true;
        Opened?.Invoke();
    }

    private void VerticalClose()
    {
        _background.SetActive(false);

        IsOpened = false;
        Closed?.Invoke();
    }


    private void HorizontalClose()
    {
        _background.SetActive(false);

        IsOpened = false;
        Closed?.Invoke();
    }
}
