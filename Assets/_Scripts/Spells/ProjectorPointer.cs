using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectorPointer : MonoBehaviour
{
    [SerializeField] private GameObject _pointerPrefab;

    private Transform _pointer;

    private void Awake()
    {
        _pointer = Instantiate(_pointerPrefab).transform;    
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

        if (hits.Length == 0)
            _pointer.gameObject.SetActive(false);

        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent(out ICardApplicable applicable))
            {
                _pointer.gameObject.SetActive(true);
                _pointer.position = hit.point;
            }
        }
    }
}
