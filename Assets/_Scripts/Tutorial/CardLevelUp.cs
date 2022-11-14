using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardLevelUp : MonoBehaviour
{
    [SerializeField] private Image _imageCard;
    [SerializeField] private Image _imageIcon;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _amountText;

    public void ChangeDrawCard(UnitCard card)
    {
        UnitCardView unitCard = card.gameObject.GetComponent<UnitCardView>();
        _imageCard.sprite = unitCard.Background;
        _imageIcon.sprite = unitCard.Icon;
        _text = unitCard.Text;
        _amountText = unitCard.AmountText;        
    }
}

