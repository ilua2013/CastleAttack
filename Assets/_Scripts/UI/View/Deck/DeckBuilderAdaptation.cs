using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBuilderAdaptation : MonoBehaviour
{
    private const float HorizontalRatio = 1.17f;

    [SerializeField] private PanelController _panelController;
    [SerializeField] private PanelOpenView _horizontalPanel;
    [SerializeField] private PanelOpenView _verticalPanel;

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

        OnOpen();
    }

    private void AdaptateToVertical()
    {
        _verticalPanel.Open();
        _horizontalPanel.Close();
    }

    private void AdaptateToHorizontal()
    {
        _horizontalPanel.Open();
        _verticalPanel.Close();
    }

    private void OnOpen()
    {
        float ratio = (float)Screen.width / Screen.height;

        if (ratio <= HorizontalRatio)
            AdaptateToVertical();
        else
            AdaptateToHorizontal();
    }

    private void OnClose()
    {
        _horizontalPanel.Close();
        _verticalPanel.Close();
    }
}
