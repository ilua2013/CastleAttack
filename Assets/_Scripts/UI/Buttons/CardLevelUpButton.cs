using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CardLevelUpButton : MonoBehaviour
{
    [SerializeField] private CardInDeckView _cardView;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if (_cardView.Card.CardSave.CanLevelUp)
            Debug.Log("LevelUp");
    }
}
