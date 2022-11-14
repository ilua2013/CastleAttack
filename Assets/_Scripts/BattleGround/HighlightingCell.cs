using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightingCell : MonoBehaviour
{
    [SerializeField] private Color _highlight;

    private Color _default;
    private MeshRenderer _renderer;
    private bool _isSelect = false;

    public bool IsSelect => _isSelect;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _default = _renderer.material.color;
    }

    public void Select()
    {
        _isSelect = true;
        _renderer.material.color = _highlight;
    }

    public void UnSelect()
    {
        _isSelect = false;
        _renderer.material.color = _default;
    }
}
