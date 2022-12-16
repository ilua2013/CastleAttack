using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardStepsAnimation : MonoBehaviour
{
    private const string AnimationScalingName = "Scaling";

    [SerializeField] private Image[] _stars;
    [SerializeField] private Animator[] _starAnimators;
    [SerializeField] private TMP_Text _coins;
    [SerializeField] private TMP_Text _killCount;
    [SerializeField] private Sprite _starSprite;
    [SerializeField] private WheelFortune _fortune;

    private CoinsRewarder _coinsRewarder;

    private void Awake()
    {
        _coinsRewarder = FindObjectOfType<CoinsRewarder>();
    }

    private void OnEnable()
    {
        _fortune.Rewarded += OnRewarded;
    }

    private void OnDisable()
    {
        _fortune.Rewarded -= OnRewarded;
    }

    public void Play(int starsCount)
    {
        //StartCoroutine(AnimateStars(starsCount, () => 
        StartCoroutine(AnimateKill(GamesStatistics.KilledEnemy, () => 
        StartCoroutine(AnimateCoins(_coinsRewarder.ReceivedReward))));
    }

    private IEnumerator AnimateStars(int count, Action onComplete = null)
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.5f);
            _stars[i].sprite = _starSprite;
            _starAnimators[i].Play(AnimationScalingName);
        }

        yield return new WaitForSeconds(1f);
        onComplete?.Invoke();
    }

    private IEnumerator AnimateCoins(int award, Action onComplete = null)
    {
        Debug.Log(award);
        float coins = 0;

        while (coins < award)
        {
            coins = Mathf.MoveTowards(coins, award, 1f);
            _coins.text = FormatCost(coins);

            yield return new WaitForEndOfFrame();
        }

        _coins.text = FormatCost(award);
        onComplete?.Invoke();
    }

    private IEnumerator AnimateKill(int killCount, Action onComplete = null)
    {
        float count = 0;

        while (count < killCount)
        {
            count = Mathf.MoveTowards(count, killCount, 1f);
            _killCount.text = FormatCost(count);

            yield return new WaitForEndOfFrame();
        }

        _killCount.text = FormatCost(killCount);
        onComplete?.Invoke();

        Debug.Log("Стоп");

    }

    private void OnRewarded(int coins)
    {
        _coins.text = FormatCost(coins);
    }

    private string FormatCost(float cost)
    {
        var format = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        format.NumberGroupSeparator = " ";
        format.NumberDecimalDigits = 0;

        return Convert.ToDecimal(cost).ToString("N", format);
    }
}
