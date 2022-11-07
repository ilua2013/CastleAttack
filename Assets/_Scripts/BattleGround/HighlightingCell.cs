using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightingCell : MonoBehaviour
{
    [SerializeField] private Color _highlight;

    private Color _default;
    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _default = _renderer.material.color;
    }

    public void Select()
    {
        _renderer.material.color = _highlight;
    }

    public void UnSelect()
    {
        _renderer.material.color = _default;
    }
}
