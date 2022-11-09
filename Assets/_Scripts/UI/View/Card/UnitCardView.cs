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
        _card.Used += OnCardUse;
        _card.CameBack += OnCardCameBack;
    }

    private void OnDisable()
    {
        _card.Used -= OnCardUse;
        _card.CameBack -= OnCardCameBack;
    }

    private void OnCardUse(int amount)
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
