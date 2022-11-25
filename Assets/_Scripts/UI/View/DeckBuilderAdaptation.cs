using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilderAdaptation : MonoBehaviour
{
    private const float HorizontalRatio = 1.17f;

    [SerializeField] private PanelController _panelController;
    [SerializeField] private GameObject _horizontalPanel;
    [SerializeField] private GameObject _verticalPanel;

    private bool _isVertical;

    private void OnEnable()
    {
        _panelController.Opened += OnOpen;
        _panelController.Closed += OnClose;
    }

    private void OnDisable()
    {
        _panelController.Opened -= OnOpen;
        _panelController.Closed -= OnClose;
    }

    private void Update()
    {
        if (!_panelController.IsOpened)
            return;

        float ratio = (float)Screen.width / Screen.height;

        if (ratio <= HorizontalRatio)
            AdaptateToVertical();
        else
            AdaptateToHorizontal();
    }

    private void AdaptateToVertical()
    {
        if (!_isVertical)
        {
            _isVertical = true;

            _verticalPanel.SetActive(true);
            _horizontalPanel.SetActive(false);
        }
    }

    private void AdaptateToHorizontal()
    {
        if (_isVertical)
        {
            _isVertical = false;

            _horizontalPanel.SetActive(true);
            _verticalPanel.SetActive(false);
        }
    }

    private void OnOpen()
    {
        float ratio = (float)Screen.width / Screen.height;

        if (ratio <= HorizontalRatio)
        {
            _isVertical = true;

            _verticalPanel.SetActive(true);
            _horizontalPanel.SetActive(false);
        }
        else
        {
            _isVertical = false;

            _horizontalPanel.SetActive(true);
            _verticalPanel.SetActive(false);
        }
    }

    private void OnClose()
    {
        _horizontalPanel.SetActive(false);
        _verticalPanel.SetActive(false);
    }
}
