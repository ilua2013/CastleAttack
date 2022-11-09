using TMPro;
using UnityEngine;

[RequireComponent(typeof(UnitCard))]
public class UnitCardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _amountText;

    private UnitCard _card;
    private string _descriptionTextBase;

    private void Awake()
    {
        _card = GetComponent<UnitCard>();
        _descriptionTextBase = _text.text;
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
        _text.text = $"{_descriptionTextBase}";
        _amountText.text = $"{_card.Amount}";
    }
}
