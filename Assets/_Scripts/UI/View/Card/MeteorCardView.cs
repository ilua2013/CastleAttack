using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeteorCardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private MeteorSpellCardDescription _description;

    private void Awake()
    {
        _text.text = $"{_description.DamagePerSecond} {_text.text}";
    }
}
