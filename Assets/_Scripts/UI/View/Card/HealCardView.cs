using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealCardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private HealSpell _description;

    private void Awake()
    {
        _text.text = $"{_description.Recovery} {_text.text}";
    }
}
