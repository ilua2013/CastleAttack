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
    [SerializeField] private Transform _arrow;
    [SerializeField] private CoinsWallet _wallet;
    [SerializeField] private Rewarder _coinsRewarder;

    private bool _isRewarded;
    private int _factor = 1;

    public event Action<int> Rewarded;

    private void OnValidate()
    {
        if (_wallet == null)
            _wallet = FindObjectOfType<CoinsWallet>(true);

        if (_coinsRewarder == null)
            _coinsRewarder = FindObjectOfType<Rewarder>(true);
    }

    private void Start()
    {
        StartCoroutine(Spin(_arrow, _speed));
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
        _wallet.Add(_coinsRewarder.ReceivedCoins * _factor, 0);
        _panel.SetAward(_coinsRewarder.ReceivedCoins * _factor);
        Rewarded?.Invoke(_coinsRewarder.ReceivedCoins * _factor);

        _panel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private IEnumerator Spin(Transform arrow, float speed)
    {
        float targetAngleX = _angleClamp.x;
        float targetAngleY = _angleClamp.y;
        Quaternion target = Quaternion.Euler(arrow.eulerAngles.x, arrow.eulerAngles.y, targetAngleX);

        while (_isRewarded == false)
        {
            if (arrow.rotation == Quaternion.Euler(arrow.eulerAngles.x, arrow.eulerAngles.y, targetAngleX))
                target = Quaternion.Euler(arrow.eulerAngles.x, arrow.eulerAngles.y, targetAngleY);

            else if (arrow.rotation == Quaternion.Euler(arrow.eulerAngles.x, arrow.eulerAngles.y, targetAngleY))
                target = Quaternion.Euler(arrow.eulerAngles.x, arrow.eulerAngles.y, targetAngleX);

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
