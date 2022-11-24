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

    private readonly Vector3 _initialScale = new Vector3(1.5f, 1.5f, 1.5f);
    private Coroutine _coroutine;
    private Card _card;

    public Card Card => _card;

    public void FillCard(Card card, bool smooth)
    {
        _card = card;

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
        _upArrow.enabled = cardSave.CanLevelUp;
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