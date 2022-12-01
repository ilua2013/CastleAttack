using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MuteButton : MonoBehaviour
{
    [SerializeField] private Sprite _mutedSprite;
    [SerializeField] private Image _image;

    private Sprite _defaultSprite;
    private Button _button;
    private bool _isMuted;

    private void Awake()
    {
        _defaultSprite = _image.sprite;
        _button = GetComponent<Button>();

        if (Saves.HasKey(SaveController.Params.IsSoundMuted))
            _isMuted = Saves.GetBool(SaveController.Params.IsSoundMuted);

        AudioListener.pause = _isMuted;
        AudioListener.volume = _isMuted ? 0 : 1;

        if (_isMuted)
            _image.sprite = _mutedSprite;
        else
            _image.sprite = _defaultSprite;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        _isMuted = !_isMuted;

        Saves.SetBool(SaveController.Params.IsSoundMuted, _isMuted);
        Saves.Save();

        AudioListener.pause = _isMuted;
        AudioListener.volume = _isMuted ? 0 : 1;

        if (_isMuted)
            _image.sprite = _mutedSprite;
        else
            _image.sprite = _defaultSprite;
    }
}
