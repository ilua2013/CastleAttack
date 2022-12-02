using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Agava.YandexGames;

[RequireComponent(typeof(TMP_Text))]
public class LocalizationText : MonoBehaviour
{
    [SerializeField] private LocalizationLabel[] _options;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _text.text = GetLabel(Localization.Instance.Language);
    }

    private string GetLabel(Language language)
    {
        foreach (var option in _options)
        {
            if (option.Language == language)
                return option.Text;
        }

        throw new InvalidOperationException("No such language found: " + nameof(language));
    }
}
