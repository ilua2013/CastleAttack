using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FinishPanel))]
public class TutorialWinRecorder : MonoBehaviour
{
    private FinishPanel _winPanel;

    private void Awake()
    {
        _winPanel = GetComponent<FinishPanel>();
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
