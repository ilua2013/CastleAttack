using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class CardMovement : MonoBehaviour
{
    [SerializeField] private Image _raycastBG;

    private const float Speed = 20f;
    private const float LerpTime = 10f;
    private const float DistanceDelta = 0.1f;
    private const float AngleClamp = 35f;
    private readonly Vector3 PlacementScale = new Vector3 (0.5f,0.5f,0.5f);

    private Vector3 _initialScale;
    private Transform _draggingParent;
    private PointerEventData _target;
    private Card _card;

    private void Awake()
    {
        _card = GetComponent<Card>();

        _initialScale = transform.localScale;
        _draggingParent = transform.parent;
    }

    private void OnEnable()
    {
        _card.BeginDrag += OnBeginMove;
        _card.EndDrag += OnEndMove;
        _card.Drop += OnDrop;
        _card.Activated += OnActivate;
    }

    private void OnDisable()
    {
        _card.BeginDrag -= OnBeginMove;
        _card.EndDrag -= OnEndMove;
        _card.Drop -= OnDrop;
        _card.Activated -= OnActivate;
    }

    public void Init(Transform dragging)
    {
        _draggingParent = dragging;
    }

    public void Move()
    {
        if (_target == null)
            return;

        Vector3 position = new Vector3(_target.position.x, _target.position.y, 0);
        transform.position = Vector3.Lerp(transform.position, position, Speed * Time.deltaTime);

        float angle = (transform.position.x - position.x);
        angle = Mathf.Clamp(angle, -AngleClamp, AngleClamp);

        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Speed * Time.deltaTime);

        transform.localScale = CalculateScale();
    }

    private Vector3 CalculateScale()
    {
        Vector3 scale = IsOverDraggingPanel(_target) ? _initialScale : PlacementScale;

        return Vector3.Lerp(transform.localScale, scale, LerpTime * Time.deltaTime);
    }

    private bool IsOverDraggingPanel(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var item in results)
        {
            if (!item.isValid)
                continue;

            if (item.gameObject.transform == _draggingParent)
                return true;
        }

        return false;
    }

    private void OnDrop(Card card, Vector3 mousePosition)
    {
        StartCoroutine(LerpScale(Vector3.zero));
    }

    private IEnumerator LerpScale(Vector3 to)
    {
        while (Vector3.Distance(transform.localScale, to) > DistanceDelta)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, to, LerpTime * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = to;
    }

    private void OnBeginMove(PointerEventData eventData, Card card)
    {
        _target = eventData;
    }

    private void OnEndMove(PointerEventData eventData, Card card)
    {
        _target = null;
    }

    private void OnActivate(bool isActive)
    {
        _raycastBG.raycastTarget = isActive;
    }
}
