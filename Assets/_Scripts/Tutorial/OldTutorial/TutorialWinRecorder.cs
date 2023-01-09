using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialWinRecorder : MonoBehaviour
{
    [SerializeField] private FinishPanel _winPanel;

    private void OnValidate()
    {
        if (_winPanel == null)
            _winPanel = FindObjectOfType<FinishPanel>();
    }

    private void Awake()
    {
        Saves.SetBool(SaveController.Params.IsTutorialCompleted, false);
        Saves.Save();
    }

    private void OnEnable()
    {
        _winPanel.Opened += OnWinPanelOpened;
    }

    private void OnDisable()
    {
        _winPanel.Opened -= OnWinPanelOpened;
    }

    private void OnWinPanelOpened()
    {
        Saves.SetBool(SaveController.Params.IsTutorialCompleted, true);
        Saves.Save();
    }
}
