using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImproveCardStats : MonoBehaviour
{
    [SerializeField] private CardLevelUpButton _improveButton;
    [SerializeField] private CardInDeckView _cardView;

    private void OnEnable()
    {
        _improveButton.LevelUp += OnLevelUp;
    }

    private void OnDisable()
    {
        _improveButton.LevelUp -= OnLevelUp;
    }

    private void OnLevelUp()
    {
        int damage = _cardView.Card.CardSave.UnitStats.Damage;
        int health = _cardView.Card.CardSave.UnitStats.MaxHealth;

        _cardView.Card.CardSave.UnitStats.Improve(damage + 1, health + 1);
        _cardView.Card.Save();
    }
}
