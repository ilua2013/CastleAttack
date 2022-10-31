using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OgreCardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private UnitStep _description;

    private void OnEnable()
    {
        _description.Inited += WriteText;
    }
    private void OnDisable()
    {
        _description.Inited -= WriteText;
    }

    private void WriteText()
    {
        _text.text = $"{_description.Fighter.Damage} {_text.text}"; // Требует корректировки
    }
}
