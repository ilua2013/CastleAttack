using System;
using UnityEngine;

public enum Language
{
    ru,
    en,
    tr
}

[Serializable]
public class LocalizationLabel
{
    [SerializeField] private string _text;
    [SerializeField] private Language _language;

    public string Text => _text;
    public Language Language => _language;
}
