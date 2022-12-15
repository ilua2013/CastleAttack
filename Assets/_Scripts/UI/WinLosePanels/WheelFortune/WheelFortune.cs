using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelFortune : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private CardRewardPanel _panel;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Vector2 _angleClamp;
    [SerializeField] private float _speed;
    [SerializeField] private Image _arrow;

    private bool _isRewarded;
    private int _factor = 1;
    private CoinsWallet _wallet;
    private CoinsRewarder _coinsRewarder;

    public event Action<int> Rewarded;

    private void Awake()
    {
        _wallet = FindObjectOfType<CoinsWallet>(true);
        _coinsRewarder = FindObjectOfType<CoinsRewarder>(true);
    }

    private void Start()
    {
        StartCoroutine(Spin(_arrow.transform, _speed));
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
        _isRewarded = true;

#if !UNITY_EDITOR
        YandexSDK.Instance.ShowVideoAd(OnRewarded);
        return;
#endif

        OnRewarded();
    }

    private void OnRewarded()
    {
        _wallet.Add(_coinsRewarder.ReceivedReward * _factor);
        _panel.SetAward(_coinsRewarder.ReceivedReward * _factor);
        Rewarded?.Invoke(_coinsRewarder.ReceivedReward * _factor);

        _panel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private IEnumerator Spin(Transform arrow, float speed)
    {
        float targetAngleX = _angleClamp.x;
        float targetAngleY = _angleClamp.y;
        Quaternion target = Quaternion.Euler(0, 0, targetAngleX);

        while (_isRewarded == false)
        {
            if (arrow.rotation == Quaternion.Euler(0, 0, targetAngleX))
                target = Quaternion.Euler(0, 0, targetAngleY);
            else if (arrow.rotation == Quaternion.Euler(0, 0, targetAngleY))
                target = Quaternion.Euler(0, 0, targetAngleX);

            arrow.rotation = Quaternion.RotateTowards(arrow.rotation, target, speed * Time.deltaTime);
            ChangeFactor(arrow.rotation);

            yield return null;
        }
    }

    private void ChangeFactor(Quaternion rotation)
    {
        float angle = Quaternion.Angle(rotation, Quaternion.identity);
        if (Mathf.Abs(angle) > 47)
            _factor = 1;
        else if (Mathf.Abs(angle) > 10 && Mathf.Abs(angle) <= 47)
            _factor = 2;
        else
            _factor = 3;

        _text.text = $"x{_factor}";
    }
}
