using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningViewedRecorder : MonoBehaviour
{
    [SerializeField] private NextLevelButton _button;

    private void OnValidate()
    {
        if (_button == null)
            _button = FindObjectOfType<NextLevelButton>();
    }

    private void OnEnable()
    {
        _button.NextLevelLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        _button.NextLevelLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded()
    {
        Saves.SetBool(SaveController.Params.IsOpeningViewed, true);
        Saves.Save();
    }
}
