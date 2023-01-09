using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsAwardCard : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    public void Init(int coins)
    {
        _text.text = coins.ToString();
    }
}
