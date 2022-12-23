using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationTextLegacy : MonoBehaviour
{
    [SerializeField] private LocalizationLabel[] _options;

    private Text _text;

    private void Awake()
    {
        _text = GetComponent<Text>();

#if UNITY_EDITOR
        _text.text = GetLabel(Language.ru);
#else
        _text.text = GetLabel(Localization.Instance.Language);
#endif
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
