using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardInDeckView : MonoBehaviour
{
    private const float LerpTime = 10f;
    private const float DistanceDelta = 0.01f;

    [SerializeField] private Image _background;
    [SerializeField] private Slider _amountBar;
    [SerializeField] private Image _upArrow;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private GameObject _coins;
    [SerializeField] private GameObject _button;
    [SerializeField] private GameObject _levelPanel;
    [SerializeField] private bool _isMoveable;

    private string _level;
    private readonly Vector3 _initialScale = new Vector3(1.5f, 1.5f, 1.5f);
    private Coroutine _coroutine;
    private Card _card;

    public Card Card => _card;
    public bool CanLevelUp => _card.CardSave.CanLevelUp;

    public bool TryLevelUpCard()
    {
        if (_card == null)
            return false;

        if (_card.CardSave.CanLevelUp)
        {
            _card.CardSave.LevelUp();
            _card.Save();
            SetInfo(_card.CardSave);

            return true;
        }

        return false;
    }

    public void Clear()
    {
        if (string.IsNullOrEmpty(_level))
            _level = _levelText.text;

        _card = null;
        _button.SetActive(false);
        _levelPanel.SetActive(false);

        if (Card != null && Card.CardSave.Deck == DeckType.Common)
            _background.gameObject.SetActive(false);

        _upArrow.gameObject.SetActive(false);
        _levelText.text = _level;
        _amountText.text = "0 / 3";
        _amountBar.value = 0;
    }

    public void FillCard(Card card, bool smooth)
    {
        _card = card;
        _card.Activate(_isMoveable);

        SetHierarchy(card.transform);
        Transformation(card.transform, smooth);
        SetInfo(card.CardSave);

        card.transform.localScale = Vector3.one;
        card.BeginDrag += OnBeginDrag;
    }

    private void SetInfo(CardSave cardSave)
    {
        _amountBar.wholeNumbers = true;
        _amountBar.minValue = 0;
        _amountBar.maxValue = cardSave.AmountToImprove;

        _amountBar.value = cardSave.Amount;
        _amountText.text = $"{cardSave.Amount} / {cardSave.AmountToImprove}";

        if (string.IsNullOrEmpty(_level))
            _level = _levelText.text;

        _levelText.text = _level + " " + cardSave.Level;
        _upArrow.gameObject.SetActive(cardSave.CanLevelUp);
        _coins.SetActive(cardSave.CanLevelUp);
        _button.SetActive(true);
        _levelPanel.SetActive(true);

        if (Card != null && Card.CardSave.Deck == DeckType.Combat)
            _background.gameObject.SetActive(true);

        _amountText.gameObject.SetActive(!cardSave.CanLevelUp);
    }

    private void SetHierarchy(Transform card)
    {
        card.SetParent(transform);
        card.SetAsFirstSibling();

        _background.transform.SetAsFirstSibling();
    }

    private void Transformation(Transform card, bool smooth)
    {
        card.localScale = _initialScale;
        card.rotation = Quaternion.identity;

        if (smooth)
            _coroutine = StartCoroutine(LerpPosition(card, Vector3.zero));
        else
            card.localPosition = Vector3.zero;
    }

    private void OnBeginDrag(PointerEventData eventData, Card card)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator LerpPosition(Transform card, Vector3 to)
    {
        while (Vector3.Distance(card.localPosition, to) > DistanceDelta)
        {
            card.localPosition = Vector3.Lerp(card.localPosition, to, LerpTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        card.localPosition = to;
    }
}