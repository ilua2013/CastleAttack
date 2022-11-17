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

    private readonly Vector3 _initialScale = new Vector3(1.5f, 1.5f, 1.5f);
    private Coroutine _coroutine;

    public void FillCard(Card card, bool smooth)
    {
        SetHierarchy(card.transform);
        Transformation(card.transform, smooth);

        card.BeginDrag += OnBeginDrag;
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
        StopCoroutine(_coroutine);
        card.transform.localPosition = Vector3.zero;
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