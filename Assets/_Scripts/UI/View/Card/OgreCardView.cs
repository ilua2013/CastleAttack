using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OgreCardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private OgreCardDescription _description;

    private void Awake()
    {
        _text.text = $"{_description.DamagePerSecond} {_text.text}";
    }
}
