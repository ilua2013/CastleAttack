using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightingCell : MonoBehaviour
{
    [SerializeField] private Color _highlight;
    [SerializeField] private Color _highlightEnemy;
    [SerializeField] private Color _highLightMiddle;

    private Color _default;
    private MeshRenderer _renderer;
    private bool _isSelect = false;
    private bool _isSelectEnemy = false;
    private bool _isSelectFriend = false;
    private int _selectCount = 0;
    private int _selectCountEnemy = 0;

    public bool IsSelect => _isSelect;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _default = _renderer.material.color;
    }

    public void Select()
    {
        _isSelect = true;
        _isSelectFriend = true;
        if (_isSelectEnemy == true && _isSelectFriend == true)
            _renderer.material.color = _highLightMiddle;
        else
        {
            _renderer.material.color = _highlight;
        }
        ++_selectCount;
    }

    public void SelectEnemy()
    {
        _isSelect = true;
        _isSelectEnemy = true;
        if (_isSelectEnemy == true && _isSelectFriend == true)
            _renderer.material.color = _highLightMiddle;
        else
        {
            _renderer.material.color = _highlightEnemy;
        }
        ++_selectCountEnemy;
    }

    public void UnSelect()
    {
        --_selectCount;
        if (_selectCount == 0)
        {
            _isSelectFriend = false;
            _renderer.material.color = _highlightEnemy;
            if (_isSelectEnemy == false)
            {
                DefaultSelect();
            }
        }

        if (_selectCount < 0)
        {
            _isSelect = false;
            _isSelectEnemy = false;
            _isSelectFriend = false;
            _renderer.material.color = _default;
            _selectCount = 0;
        }
    }
    public void UnSelectEnemy()
    {
        --_selectCountEnemy;
        if (_selectCountEnemy == 0)
        {
            _isSelectEnemy = false;
            _renderer.material.color = _highlight;
            if (_isSelectFriend == false)
            {
                DefaultSelect();
            }
        }
    }

    private void DefaultSelect()
    {
        _isSelect = false;
        _renderer.material.color = _default;
    }
}
