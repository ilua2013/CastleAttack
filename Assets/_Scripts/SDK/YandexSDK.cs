using System;
using System.Collections;
using System.Collections.Generic;
using Agava.WebUtility;
using Agava.YandexGames;
using UnityEngine;

public class YandexSDK : MonoBehaviour
{
    public static YandexSDK Instance = null;

    public event Action Initialized;

    public bool IsInitialize { get; private set; }
    public Agava.YandexGames.DeviceType DeviceType { get; private set; } = Agava.YandexGames.DeviceType.Desktop;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
    }

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        IsInitialize = true;
        Initialized?.Invoke();
        yield break;
#endif

        yield return YandexGamesSdk.Initialize(Initialize);
    }

    private void Initialize()
    {
        DeviceType = Device.Type;
        IsInitialize = true;
        Initialized?.Invoke();
    }

    public void ShowInterstitial(Action onOpenCallback = null, Action onCloseCallback = null)
    {
        void onOpenAction()
        {
            onOpenCallback?.Invoke();
            MuteAudio(true);
        }

        void onCloseAction(bool wasShown)
        {
            onCloseCallback?.Invoke();
            MuteAudio(false);
        }

#if UNITY_EDITOR
        onOpenAction();
        onCloseAction(true);
        return;
#endif

        InterstitialAd.Show(onOpenAction, onCloseAction);
    }

    public void ShowVideoAd(Action onRewardedCallback = null, Action onOpenCallback = null, Action onCloseCallback = null)
    {
        void onOpenAction()
        {
            onOpenCallback?.Invoke();
            MuteAudio(true);
        }

        void onCloseAction()
        {
            onCloseCallback?.Invoke();
            MuteAudio(false);
        }

        void onRewardedAction()
        {
            onRewardedCallback?.Invoke();
        }

#if UNITY_EDITOR
        onOpenAction();
        onRewardedAction();
        onCloseAction();
        return;
#endif

        VideoAd.Show(onOpenAction, onRewardedAction, onCloseAction);
    }

    private void OnInBackgroundChange(bool inBackground)
    {
        MuteAudio(inBackground);
    }

    private void MuteAudio(bool value)
    {
        if (Saves.HasKey(SaveController.Params.IsSoundMuted))
            if (Saves.GetBool(SaveController.Params.IsSoundMuted))
                return;

        AudioListener.pause = value;
        AudioListener.volume = value ? 0f : 1f;
    }
}
