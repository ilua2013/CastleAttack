using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartFightButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _imageButton;
    [SerializeField] private FightSystem _fightSystem;

    private void OnEnable()
    {
        _fightSystem.StepFinished += EnableButton;
        _fightSystem.StepStarted += DisableButton;
    }
    private void OnDisable()
    {
        _fightSystem.StepFinished -= EnableButton;
        _fightSystem.StepStarted -= DisableButton;
    }

    private void EnableButton()
    {
        _button.enabled = true;
        _imageButton.color = Color.yellow;
    }

    private void DisableButton()
    {
        _button.enabled = false;
        _imageButton.color = Color.gray;
    }
}
