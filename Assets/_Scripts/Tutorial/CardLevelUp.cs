using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardLevelUp : MonoBehaviour
{
    [SerializeField] private Image _imageCard;
    [SerializeField] private Image _imageIcon;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private Transform _transform;

    public void ChangeDrawCard(UnitCard card)
    {
        Instantiate(card, _transform);
    }

    public void ChangeOldCard(UnitCard card)
    {
        UnitCard cardOld = Instantiate(card, _transform);
        cardOld.gameObject.SetActive(true);
        cardOld.transform.localPosition = Vector3.zero;
    }
}
