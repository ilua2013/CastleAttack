using Agava.YandexGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Localization : MonoBehaviour
{
    public static Localization Instance = null;

    public Language Language { get; private set; }

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
        YandexSDK.Instance.Initialized += OnInitialize;
    }

    private void OnDisable()
    {
        YandexSDK.Instance.Initialized -= OnInitialize;
    }

    private void OnInitialize()
    {
        string domain = "ru";

#if UNITY_EDITOR
        return;
#endif

        domain = YandexGamesSdk.Environment.i18n.lang;

        switch (domain)
        {
            case "ru":
                Language = Language.ru;
                break;

            case "en":
                Language = Language.en;
                break;

            case "tr":
                Language = Language.tr;
                break;

            default:
                throw new ArgumentException("No such language found " + nameof(domain));
        }
    }
}
