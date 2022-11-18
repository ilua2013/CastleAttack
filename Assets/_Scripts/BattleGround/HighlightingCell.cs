using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightingCell : MonoBehaviour
{
    [SerializeField] private GameObject _highlightFriend;
    [SerializeField] private GameObject _highlightEnemy;
    [SerializeField] private GameObject _highLightMiddle;
    [SerializeField] private GameObject _highlightHalf;

   
    private MeshRenderer _renderer;
    private bool _isSelect = false;
    private bool _isSelectEnemy = false;
    private bool _isSelectFriend = false;
    private int _selectCount = 0;
    private int _selectCountEnemy = 0;

    public bool IsSelect => _isSelect;

    private void Awake()
    {
        _highlightEnemy.SetActive(false);
        _highlightFriend.SetActive(false);
        _highlightHalf.SetActive(false);
        _highLightMiddle.SetActive(false);
        _renderer = GetComponent<MeshRenderer>();
        //_default = _renderer.material.color;
    }

    public void Select()
    {
        _isSelect = true;
        _isSelectFriend = true;
        if (_isSelectEnemy == true && _isSelectFriend == true)
        {
            _highlightHalf.SetActive(true);
            _highLightMiddle.SetActive(true);
        }      
        else
        {
            _highlightFriend.SetActive(true);
        }
        ++_selectCount;
    }

    public void SelectEnemy()
    {
        _isSelect = true;
        _isSelectEnemy = true;
        if (_isSelectEnemy == true && _isSelectFriend == true)
        {
            _highlightHalf.SetActive(true);
            _highLightMiddle.SetActive(true);
        }
          
        else
        {
            _highlightEnemy.SetActive(true);
        }
        ++_selectCountEnemy;
    }

    public void UnSelect()
    {
        --_selectCount;
        if (_selectCount == 0)
        {
            _isSelectFriend = false;
            _highlightEnemy.SetActive(true);
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

            _highlightEnemy.SetActive(false);
            _highlightFriend.SetActive(false);
            _highlightHalf.SetActive(false);
            _highLightMiddle.SetActive(false);
            _selectCount = 0;
        }
    }
    public void UnSelectEnemy()
    {
        --_selectCountEnemy;
        if (_selectCountEnemy == 0)
        {
            _isSelectEnemy = false;
            _highlightFriend.SetActive(true);
            if (_isSelectFriend == false)
            {
                DefaultSelect();
            }
        }
    }

    private void DefaultSelect()
    {
        _isSelect = false;
        _highlightEnemy.SetActive(false);
        _highlightFriend.SetActive(false);
        _highlightHalf.SetActive(false);
        _highLightMiddle.SetActive(false);
    }
}
