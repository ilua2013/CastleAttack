using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HighlightingCell : MonoBehaviour
{
    [SerializeField] private GameObject _highlightFriend;
    [SerializeField] private GameObject _highlightEnemy;
    [SerializeField] private GameObject _highLightMiddle;
    [SerializeField] private GameObject _highlightHalf;
    [SerializeField] private GameObject _highlightBlue;


    private MeshRenderer _renderer;
    private bool _isSelect = false;
    private bool _isSelectEnemy = false;
    private bool _isSelectFriend = false;
    private int _selectCountFriend = 0;
    private int _selectCountEnemy = 0;
    private bool _isSelectSpell = false;
    private int _selectCountSpell = 0;
    private int _switchHighlight = 0;

    public bool IsSelect => _isSelect;

    private void Awake()
    {
        _highlightEnemy.SetActive(false);
        _highlightFriend.SetActive(false);
        _highlightHalf.SetActive(false);
        _highLightMiddle.SetActive(false);
        _highlightBlue.SetActive(false);
        _renderer = GetComponent<MeshRenderer>();
        //_default = _renderer.material.color;
    }

    public void SelectFriend()
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
        ++_selectCountFriend;
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

    public void SelectSpell()
    {
        _isSelect = true;
        _isSelectSpell = true;       
        _switchHighlight = 0;
        _highlightBlue.SetActive(true);
        if (_isSelectEnemy == true && _isSelectFriend == true)
        {
            _highlightHalf.SetActive(false);
            _highLightMiddle.SetActive(false);
            _highlightFriend.SetActive(false);
            _highlightEnemy.SetActive(false);
            _switchHighlight = 1;            
        }
        if (_isSelectEnemy == false && _isSelectFriend == true)
        {
            _highlightHalf.SetActive(false);
            _highLightMiddle.SetActive(false);
            _highlightFriend.SetActive(false);
            _highlightEnemy.SetActive(false);
            _switchHighlight = 2;
        }
        if(_isSelectEnemy == true && _isSelectFriend == false)
        {
            _highlightHalf.SetActive(false);
            _highLightMiddle.SetActive(false);
            _highlightFriend.SetActive(false);
            _highlightEnemy.SetActive(false);
            _switchHighlight = 3;
        }
        ++_selectCountSpell;       
    }

    public void UnSelectFriend()
    {
        --_selectCountFriend;
        if (_selectCountFriend == 0)
        {
            _isSelectFriend = false;
            //_highlightEnemy.SetActive(true);
            if (_isSelectEnemy == false&&_isSelectSpell==false)
            {
                DefaultSelect();
            }
        }

        if (_selectCountFriend < 0)
        {
            _isSelect = false;
            _isSelectEnemy = false;
            _isSelectFriend = false;

            _highlightEnemy.SetActive(false);
            _highlightFriend.SetActive(false);
            _highlightHalf.SetActive(false);
            _highLightMiddle.SetActive(false);
            _selectCountFriend = 0;
        }
    }

    public void UnSelectSpell()
    {
        --_selectCountSpell;
        if(_selectCountSpell == 0)
        {
            _isSelectSpell = false;
            _highlightBlue.SetActive(false);
           
            switch (_switchHighlight)
            {
                case 1:
                    _highlightHalf.SetActive(true);
                    _highLightMiddle.SetActive(true);
                    break;
                case 2:
                    _highlightFriend.SetActive(true);
                    break;
                case 3:
                    _highlightEnemy.SetActive(true);
                    break;             
            }
            
            if (_isSelectFriend == false&&_isSelectEnemy == false)
            {
                DefaultSelect();
            }
        }
    }

    public void UnSelectEnemy()
    {
        --_selectCountEnemy;
        if (_selectCountEnemy == 0)
        {
            _isSelectEnemy = false;
            //_highlightFriend.SetActive(true);
            if (_isSelectFriend == false && _isSelectSpell == false)
            {
                DefaultSelect();
            }
        }
    }

   private void DefaultSelect()
    {
        _isSelect = false;
        _selectCountFriend = 0;
        _selectCountEnemy = 0;
        _selectCountSpell = 0;
        _highlightEnemy.SetActive(false);
        _highlightBlue.SetActive(false);
        _highlightFriend.SetActive(false);
        _highlightHalf.SetActive(false);
        _highLightMiddle.SetActive(false);
    }
}
