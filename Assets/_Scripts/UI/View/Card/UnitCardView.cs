using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class UnitCardView : MonoBehaviour
{
    [SerializeField] private List<CardUpgradeView> _cardUpgradeView;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Image _background;
    [SerializeField] private Image _icon;

    private Card _card;
    private string _descriptionTextBase;

    private void Awake()
    {
        _card = GetComponent<Card>();
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
        CardUpgradeView cardUpgradeView = _cardUpgradeView.First((view) => view.Stage == card.Stage);

        Debug.Log(cardUpgradeView.Stage);
        AssignSprites(cardUpgradeView.Background, cardUpgradeView.Icon);
        WriteText();
    }

    private void WriteText()
    {
        _text.text = $"{_descriptionTextBase}";
        _amountText.text = $"{_card.Amount}";
    }

    private void AssignSprites(Sprite background, Sprite icon)
    {
        _background.sprite = background;
        _icon.sprite = icon;
    }
}
