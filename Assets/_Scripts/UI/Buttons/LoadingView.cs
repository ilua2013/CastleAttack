using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(NextLevelButton))]
public class LoadingView : MonoBehaviour
{
    [SerializeField] private TMP_Text _loadedText;
    [SerializeField] private TMP_Text _playText;

    private NextLevelButton _button;

    private void Awake()
    {
        _button = GetComponent<NextLevelButton>();

        _loadedText.enabled = false;
        _playText.enabled = true;
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
        _loadedText.enabled = true;
        _playText.enabled = false;

        _button.Button.interactable = false;
    }
}
