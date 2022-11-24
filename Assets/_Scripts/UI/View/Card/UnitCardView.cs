using TMPro;
using UnityEngine;

[RequireComponent(typeof(UnitCard))]
public class UnitCardView : CardView
{
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private TMP_Text _healthText;

    private UnitCard _card;
    private string _descriptionTextBase;

    private void Awake()
    {
        _card = GetComponent<UnitCard>();
        _descriptionTextBase = Text.text;
        WriteText();
    }

    private void OnEnable()
    {
        _card.Used += OnAmountChange;
        _card.CameBack += OnCardCameBack;
        _card.AmountChanged += OnAmountChange;
    }

    private void OnDisable()
    {
        _card.Used -= OnAmountChange;
        _card.CameBack -= OnCardCameBack;
        _card.AmountChanged -= OnAmountChange;
    }

    private void OnAmountChange(int amount)
    {
        WriteText();
    }

    private void OnCardCameBack(Card card)
    {
        WriteText();
    }

    private void WriteText()
    {
        _damageText.text = _card.UnitPrefab.Fighter.Damage.ToString();
        _healthText.text = _card.UnitPrefab.Fighter.MaxHealth.ToString();

        Text.text = $"{_descriptionTextBase}";
        AmountText.text = $"{_card.Amount}";
    }
}
